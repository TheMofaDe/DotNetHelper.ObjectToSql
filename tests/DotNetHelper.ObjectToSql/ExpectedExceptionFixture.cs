
using System;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Model;
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


        [Test]
        public void Test_Ensure_MissingKeyException_Is_Not_Thrown_With_Specified_In_Expression()
        {

            void Execute<T>(DataBaseType type, T instance, ActionType actionType, params Expression<Func<T, object>>[] keyFields) where T : class
            {
                var sql = new Services.ObjectToSql(type).BuildQuery<T>(actionType, "Test", keyFields);

            };

            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var employee2 = new // have to not 
                {
                    FirstName = "",
                    LastName = "",
                    DOB = DateTime.Now,
                    FavoriteColor = "RED",
                    CreatedAt = DateTime.Now

                };
                Execute(type, employee2, ActionType.Insert, o => o.CreatedAt);
            });
        }




        //[Test]
        //public void Test_BuildQuery_Overload_With_Dynamic_Object_Throws_InvalidOperationException()
        //{

        //    dynamic dynamicObj = new ExpandoObject();
        //    dynamicObj.Name = "dsf";
        //    var obj2Sql = new Services.ObjectToSql(DataBaseType.SqlServer);

        //    Assert.That(() => obj2Sql.BuildQuery(ActionType.Update,dynamicObj,"Name"));
        //        Throws.Exception
        //            .TypeOf<InvalidOperationException>();

        //        Assert.That(() => obj2Sql.BuildQuery(ActionType.Upsert, dynamicObj, "Name"));
        //        Throws.Exception
        //            .TypeOf<InvalidOperationException>();

        //        Assert.That(() => obj2Sql.BuildQuery(ActionType.Delete, dynamicObj, "Name"));
        //        Throws.Exception
        //            .TypeOf<InvalidOperationException>();


        //}



        public void EnsureGettingTableDoesntResultInInfiniteLoop()
        {


        }

    }
}
