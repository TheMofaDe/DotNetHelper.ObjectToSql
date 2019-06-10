using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Anonymous.Insert
{
    public class SqlServerAnonymousInsertFixture
    {

        public StringBuilder StringBuilder { get; set; }
        
        [SetUp]
        public void Setup()
        {
            StringBuilder = new StringBuilder();
        }
        [TearDown]
        public void Teardown()
        {
            StringBuilder.Clear();
        }


        [Test]
        public void Test_Anonymous_BuildInsertQuery()
        {
            var obj = new { FirstName = 1, LastName = "sfsd"};
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery(StringBuilder, "Table",obj.GetType());
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Table ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }

        [Test]
        public void Test_Anonymous_BuildInsertQuery_uses_type_name_when_null()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery<Employee>(StringBuilder, null);
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }



        [Test]
        public void Test_Anonymous_BuildInsertQueryWithOutputs()
        {
            var stringBuilder = new StringBuilder();
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

            sqlServerObjectToSql.BuildInsertQueryWithOutputs<Employee>(stringBuilder, nameof(Employee),e =>e.FirstName  );
            var sql = stringBuilder.ToString();
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName)");
        }


        






    }
}