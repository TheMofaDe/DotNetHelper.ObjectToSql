using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.DataTableToSql
{
    class DataTableToSqlFixture : BaseTest
    {

        public DataTable MockDataIdentityKey { get; } = GetMockDataIdentityKey();

        private static DataTable GetMockDataIdentityKey()
        {
            var dt = new DataTable("EmployeeWithIdentityKeySqlColumn");
            dt.Columns.Add("IdentityKey", typeof(int));
            dt.Columns["IdentityKey"].AutoIncrement = true;
            dt.PrimaryKey = new[] { dt.Columns["IdentityKey"] };
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Rows.Add(1, "John", "Doe");

            return dt;
        }

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Test_ConvertSQLToReadable()
        {
            var dt2Sql = new Services.DataTableToSql(DataBaseType.SqlServer);

            // create an datatable you want to convert to sql
            var dt = new DataTable("Employee");
            dt.Columns.Add("IdentityKey", typeof(int));
            dt.Columns["IdentityKey"].AutoIncrement = true;
            dt.PrimaryKey = new[] { dt.Columns["IdentityKey"] };
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Rows.Add(1, "John", "Doe");

            // create dbparameters from my object
            var dbParameters = dt2Sql.BuildDbParameterList(dt.Rows[0], (s, o) => new SqlParameter(s, o));

            // create my parameterized sql based on my specified action type
            var insertSql = dt2Sql.BuildQuery(dt, ActionType.Insert);

            // convert my parameterize sql to be readable
            var readAble = dt2Sql.SqlSyntaxHelper.ConvertParameterSqlToReadable(dbParameters, insertSql, Encoding.UTF8);
            // unit test
            Assert.AreEqual(readAble, "INSERT INTO Employee ([FirstName],[LastName]) VALUES ('John','Doe')");

        }

        [Test]
        public void Test_Object2Sql_ConvertSQLToReadable()
        {

            var obj2Sql = new Services.ObjectToSql(DataBaseType.SqlServer);
            // create an object you want to convert to sql
            var employee = new Employee();

            // create dbparameters from my object
            var dbParameters = obj2Sql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o));
            // create my parameterized sql based on my specified action type
            var insertSql = obj2Sql.BuildQuery<Employee>(ActionType.Insert);
            // convert my parameterize sql to be readable
            var readAble = obj2Sql.SqlSyntaxHelper.ConvertParameterSqlToReadable(dbParameters, insertSql, Encoding.UTF8);
            // unit test
            Assert.AreEqual(readAble, "INSERT INTO Employee ([FirstName],[LastName]) VALUES (NULL,NULL)");
        }

        [Test]
        public void Test_Anonymous_T_BuildQuery_With_Specified_Table_Name()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var dt2Sql = new Services.DataTableToSql(type);

                var insertSql = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Insert);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Insert, type), insertSql, "DataTable 2 Sql insertSql Failed");

                var updateSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Update);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Update, type), updateSQL, "DataTable 2 Sql updateSQL Failed");

                var deleteSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Delete);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Delete, type), deleteSQL, "DataTable 2 Sql deleteSQL Failed");

                if (type == DataBaseType.Sqlite || type == DataBaseType.MySql) return; // TODO :: fIX for sqlite
                // TODO :: FIX 
                var upsertSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Upsert);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Upsert, type), upsertSQL, "DataTable 2 Sql upsertSQL Failed");
            });

        }


    }
}
