using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Upsert
{
    public class SqlServerGenericUpsertFixture : BaseTest
    {

        public ActionType ActionType { get; } = ActionType.Upsert;
        public List<RunTimeAttributeMap> RunTimeAttribute { get; } = new List<RunTimeAttributeMap>()
        {
            new RunTimeAttributeMap("PrimaryKey",new List<System.Attribute>(){new KeyAttribute()})
        };

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Test_Generic_Build_Upsert_Query()
        {
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => objectToSql.BuildQuery<Employee>(ActionType),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }

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