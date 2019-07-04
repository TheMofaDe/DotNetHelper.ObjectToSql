using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Anonymous.Insert
{
    public class SqliteAnonymousInsertFixture
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
        public void Test_Anonymous_BuildInsertQuery()
        {
            var obj = new { FirstName = 1, LastName = "sfsd"};
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery( "Table",ActionType.Insert,obj);
            Assert.AreEqual(sql, "INSERT INTO Table ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }

        [Test]
        public void Test_Anonymous_BuildInsertQuery_uses_type_name_when_null()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<Employee>( null,ActionType.Insert);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }



        //[Test]
        //public void Test_Anonymous_BuildInsertQueryWithOutputs()
        //{
        //    var stringBuilder = new StringBuilder();
        //    var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);

        //    SqliteObjectToSql.BuildInsertQueryWithOutputs<Employee>(stringBuilder, nameof(Employee),e =>e.FirstName  );
        //    var sql = stringBuilder.ToString();
        //    Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName)");
        //}


        






    }
}