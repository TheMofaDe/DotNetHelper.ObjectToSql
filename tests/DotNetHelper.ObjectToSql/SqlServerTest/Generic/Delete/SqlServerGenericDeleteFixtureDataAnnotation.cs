using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Delete
{
    public class SqlServerGenericDeleteFixtureDataAnnotation : BaseTest
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
        public void Test_BuildQuery_Generic_As_Object_Overload_Throws_With_Key_Attribute_Decorated()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                object employee = new Employee();
                var objectToSql = new Services.ObjectToSql(type);
                Assert.That(() => objectToSql.BuildQuery(ActionType, employee),
                    Throws.Exception
                        .TypeOf<MissingKeyAttributeException>());
            });
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithManyPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithManyPrimaryKeyDataAnnotation.ToSql(ActionType, type));
            });
        }


        //[Test]
        //public void Test_Generic_BuildDeleteQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var objectToSql = new Services.ObjectToSql(type);
        //    objectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee), e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [FirstName]=@FirstName");
        //}



    }
}