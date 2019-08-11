using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Insert
{
    public class SqliteGenericInsertFixtureSqlColumn
    {
        public ActionType ActionType { get; } = ActionType.Insert;

        [SetUp]
        public void Setup()
        {
           
        }
        [TearDown]
        public void Teardown()
        {
            
        }
    



        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>( ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnSqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType, SqliteObjectToSql.DatabaseType));
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIgnorePropertySqlColumn>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertySqlColumn.ToSql(ActionType));
            
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        }



        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(
                ActionType,null, a => a.PrimaryKey);
            Assert.AreEqual(sql, $@"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) 
 OUTPUT INSERTED.[PrimaryKey] 
 VALUES (@FirstName,@LastName,@PrimaryKey)");
        }





        [Test]
        public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);

            var sql = SqliteObjectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>( ActionType,null, e => e.FirstName);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        }




    }
}