using System;
using System.Dynamic;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Update
{
    public class SqliteGenericUpdateFixtureSqlColumn
    {

        public ActionType ActionType { get; } = ActionType.Update;

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>( nameof(EmployeeWithMappedColumnAndPrimaryKeySqlColumn),ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType,SqliteObjectToSql.DatabaseType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIgnorePropertyAndKeySqlColumn>(nameof(EmployeeWithIgnorePropertyAndKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyAndKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(nameof(EmployeeWithIdentityKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithTableAttribute>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(nameof(EmployeeWithIdentityKeySqlColumn), ActionType,column => column.IdentityKey);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>(nameof(EmployeeWithManyPrimaryKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType));
        }


        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(nameof(Employee),
                ActionType, a => a.PrimaryKey);
            Assert.AreEqual(sql, $@"UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName OUTPUT DELETED.[PrimaryKey]  WHERE [PrimaryKey]=@PrimaryKey");
        }

        //[Test]
        //public void Test_Generic_BuildUpdateQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
        //    var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType, null);
        //    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));
        //}




    }
}