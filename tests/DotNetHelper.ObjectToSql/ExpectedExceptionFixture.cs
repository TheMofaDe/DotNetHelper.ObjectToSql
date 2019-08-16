
using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests
{
    public class ExpectedExceptionFixture
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
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer, false);
            //  var isInvalid = System.Enum.TryParse<ActionType>("Insert",out var invalidEnum);
            var invalidEnum = (ActionType)70;

            Assert.That(() => sqlServerObjectToSql.BuildQuery<Employee>(invalidEnum),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => sqlServerObjectToSql.BuildQuery(invalidEnum, new Employee()),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => sqlServerObjectToSql.BuildQuery<Employee>(invalidEnum, "Employee", e => e.FirstName),
                Throws.Exception
                    .TypeOf<ArgumentOutOfRangeException>());



        }


        [Test]
        public void Test_All_Build_Query_Overloads_Throws_ArgumentOutOfRange_When_Passing_Null_Parameter()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer, false);
            //  var isInvalid = System.Enum.TryParse<ActionType>("Insert",out var invalidEnum);
            var invalidEnum = (ActionType)70;


            Assert.That(() => sqlServerObjectToSql.BuildQuery(invalidEnum, null),
                Throws.Exception
                    .TypeOf<ArgumentNullException>());






        }





        public void EnsureGettingTableDoesntResultInInfiniteLoop()
        {


        }

    }
}
