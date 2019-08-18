
using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests
{
    public class ExpectedExceptionFixture : BaseTest
    {

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }


        [Test]
        public void Test_All_Build_Query_Overloads_Throws_ArgumentOutOfRange_When_Passing_Invalid_Action_Type()
        {
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer, false);
            //  var isInvalid = System.Enum.TryParse<ActionType>("Insert",out var invalidEnum);
            var invalidEnum = (ActionType)70;

            Assert.That(() => objectToSql.BuildQuery<Employee>(invalidEnum),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => objectToSql.BuildQuery(invalidEnum, new Employee()),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => objectToSql.BuildQuery<Employee>(invalidEnum, "Employee", e => e.FirstName),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());
        }


        [Test]
        public void Test_All_Build_Query_Overloads_Throws_ArgumentOutOfRange_When_Passing_Null_Parameter()
        {
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer, false);
            //  var isInvalid = System.Enum.TryParse<ActionType>("Insert",out var invalidEnum);
            var invalidEnum = (ActionType)70;


            Assert.That(() => objectToSql.BuildQuery(invalidEnum, null),
                Throws.Exception
                    .TypeOf<ArgumentNullException>());
        }



        [Test]
        public void Test_BuildQueryWithOutputs_WithNoOutput_Throws_EmptyArgumentException()
        {

            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                if (type == DataBaseType.Sqlite)
                {
                    Assert.That(() => new Services.ObjectToSql(type).BuildQueryWithOutputs<Employee>(ActionType.Insert),
                        Throws.Exception
                            .TypeOf<NotImplementedException>());
                }
                else
                {
                    Assert.That(() => new Services.ObjectToSql(type).BuildQueryWithOutputs<Employee>(ActionType.Insert),
                        Throws.Exception
                            .TypeOf<EmptyArgumentException>());
                }

            });
        }




        public void EnsureGettingTableDoesntResultInInfiniteLoop()
        {


        }

    }
}
