using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Delete
{
    public class SqlServerGenericDeleteFixtureSqlColumn : BaseTest
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
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                Assert.That(() => sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>(ActionType),
                    Throws.Exception
                        .TypeOf<MissingKeyAttributeException>());
            });
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithTableAttribute>(ActionType);
                Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType, null, column => column.IdentityKey);
                Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType, type));
            });
        }

        //[Test]
        //public void Test_Generic_BuildDeleteQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(type);
        //    var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>( ActionType, null);
        //    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType,type));

        //    sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee), e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [FirstName]=@FirstName");
        //}



    }
}