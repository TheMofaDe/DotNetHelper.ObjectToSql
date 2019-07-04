using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Update
{
    public class SqliteGenericUpdateFixture
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

        //public void Test_Generic_BuildUpdateQuery_Ensure_MissingKeyException_Is_Thrown()
        //{
        //    var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
        //    Assert.That(() => SqliteObjectToSql.BuildQuery<Employee>( nameof(Employee),ActionType,null),
        //        Throws.Exception
        //            .TypeOf<MissingKeyAttributeException>());
        //}

        [Test]
        public void Test_BuildQuery_Generic_As_Object_Overload_Throws_With_Key_Attribute_Decorated()
        {
            object employee = new Employee();
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            Assert.That(() => SqliteObjectToSql.BuildQuery("Employee", ActionType, employee),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }


    }
}