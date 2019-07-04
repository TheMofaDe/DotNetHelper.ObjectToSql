//using System.Text;
//using DotNetHelper.ObjectToSql.Enum;
//using DotNetHelper.ObjectToSql.Tests.FakeCoverage;
//using NUnit.Framework;

//namespace DotNetHelper.ObjectToSql.SqlServerTest.Anonymous
//{
//    public class SqliteTests
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test_Generic_BuildGetQuery()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new FastMember.ObjectToSql.Helper.ObjectToSql(DataBaseType.Sqlite);
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder,"Employee",null);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee ");
//        }

//        [Test]
//        public void Test_Generic_BuildGetQuery_WithWhereClause()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new FastMember.ObjectToSql.Helper.ObjectToSql(DataBaseType.Sqlite);
//            var whereClause = $"WHERE FirstName like '%Joseph%'";
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder, "Employee",  whereClause);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee " + whereClause);
//        }


//        [Test]
//        public void Test_Generic_BuildGetQuery_WithWhereClause_And_NullTableName()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new FastMember.ObjectToSql.Helper.ObjectToSql(DataBaseType.Sqlite);
//            var whereClause = $"WHERE FirstName like '%Joseph%'";
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder, null, whereClause);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee " + whereClause);
//        }
//    }
//}