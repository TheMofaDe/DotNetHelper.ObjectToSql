using System;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Delete
{
    public class SqliteGenericDeleteFixtureSqlColumn
    {

        public ActionType ActionType { get; } = ActionType.Delete;

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {
     
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Ensure_MissingKeyException_Is_Thrown_()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            Assert.That(() => SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>( nameof(EmployeeWithMappedColumnSqlColumn),ActionType),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>( nameof(EmployeeWithIdentityKeySqlColumn),ActionType);
            Assert.AreEqual(sql,EmployeeWithIdentityKeySqlColumn.ToSql(ActionType) );
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithTableAttribute>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>(nameof(EmployeeWithManyPrimaryKeySqlColumn), ActionType);
            Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(nameof(EmployeeWithIdentityKeySqlColumn), ActionType, column => column.IdentityKey);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));
        }

        //[Test]
        //public void Test_Generic_BuildDeleteQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
        //    var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType, null);
        //    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        //    SqliteObjectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee), e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [FirstName]=@FirstName");
        //}



    }
}