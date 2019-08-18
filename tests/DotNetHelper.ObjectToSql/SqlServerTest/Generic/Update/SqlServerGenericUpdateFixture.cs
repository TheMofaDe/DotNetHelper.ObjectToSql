using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
    public class SqlServerGenericUpdateFixture
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
        //    var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    Assert.That(() => objectToSql.BuildQuery<Employee>(ActionType,null),
        //        Throws.Exception
        //            .TypeOf<MissingKeyAttributeException>());
        //}

        [Test]
        public void Test_BuildQuery_Generic_As_Object_Overload_Throws_With_Key_Attribute_Decorated()
        {
            object employee = new Employee();
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => objectToSql.BuildQuery(ActionType, employee),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }


    }
}