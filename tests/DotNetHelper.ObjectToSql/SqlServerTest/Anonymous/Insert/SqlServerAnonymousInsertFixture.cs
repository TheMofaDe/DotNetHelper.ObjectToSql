using DotNetHelper.ObjectToSql.Enum;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Anonymous.Insert
{
    public class SqlServerAnonymousInsertFixture : BaseTest
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
        public void Test_Anonymous_BuildInsertQuery_With_Specified_Table_Name()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var obj = new { FirstName = 1, LastName = "sfsd" };
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery(ActionType.Insert, obj, "Table");
                Assert.AreEqual(sql, "INSERT INTO Table ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
            });
        }






    }
}