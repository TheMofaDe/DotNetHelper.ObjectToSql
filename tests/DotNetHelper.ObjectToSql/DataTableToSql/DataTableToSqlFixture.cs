using System;
using System.Collections.Generic;
using System.Data;
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
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Rows.Add(1, "Ivan", "dsfdsf");
            dt.PrimaryKey = new[] { dt.Columns["IdentityKey"] };
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
        public void Test_Anonymous_T_BuildQuery_With_Specified_Table_Name()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var dt2Sql = new Services.DataTableToSql(type);

                var insertSql = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Insert);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Insert,type),insertSql, "DataTable 2 Sql insertSql Failed");

                var updateSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Update);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Update, type), updateSQL, "DataTable 2 Sql updateSQL Failed");

                var deleteSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Delete);
                Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Delete, type), deleteSQL, "DataTable 2 Sql deleteSQL Failed");

                // TODO :: FIX 
                //var upsertSQL = dt2Sql.BuildQuery(MockDataIdentityKey, ActionType.Upsert);
                //Assert.AreEqual(EmployeeWithIdentityKeySqlColumn.ToSql(ActionType.Upsert, type), upsertSQL, "DataTable 2 Sql upsertSQL Failed");
            });

        }
    }
}
