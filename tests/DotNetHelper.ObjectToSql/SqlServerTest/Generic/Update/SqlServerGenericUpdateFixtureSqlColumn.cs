using System;
using System.Dynamic;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
    public class SqlServerGenericUpdateFixtureSqlColumn
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
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(ActionType, nameof(EmployeeWithMappedColumnAndPrimaryKeySqlColumn));
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType, sqlServerObjectToSql.DatabaseType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertyAndKeySqlColumn>( ActionType, nameof(EmployeeWithIgnorePropertyAndKeySqlColumn));
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyAndKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>( ActionType, nameof(EmployeeWithIdentityKeySqlColumn));
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithTableAttribute>( ActionType);
            Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>( ActionType, nameof(EmployeeWithIdentityKeySqlColumn), column => column.IdentityKey);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>( ActionType, nameof(EmployeeWithPrimaryKeySqlColumn));
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>( ActionType,nameof(EmployeeWithManyPrimaryKeySqlColumn));
            Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType));
        }


        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(
                ActionType, nameof(Employee), a => a.PrimaryKey);
            Assert.AreEqual(sql, $@"UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName OUTPUT DELETED.[PrimaryKey]  WHERE [PrimaryKey]=@PrimaryKey");
        }

        //[Test]
        //public void Test_Generic_BuildUpdateQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType, null);
        //    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));
        //}




    }
}