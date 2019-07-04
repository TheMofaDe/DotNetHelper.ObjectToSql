using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Upsert
{
    public class SqliteGenericUpsertFixture
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
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            Assert.That(() => SqliteObjectToSql.BuildQuery<Employee>("Employee", ActionType),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }

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