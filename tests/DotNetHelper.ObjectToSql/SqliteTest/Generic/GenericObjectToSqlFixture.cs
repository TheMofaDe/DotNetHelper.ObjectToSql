//using System.Text;
//using DotNetHelper.FastMember.ObjectToSql.Enum;
//using DotNetHelper.FastMember.ObjectToSql.Helper;
//using DotNetHelper.ObjectToSql;
//using NUnit.Framework;

//namespace Tests
//{
//    public class AnonmyousObjectToSqlFixture
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }

//        [Test]
//        public void Test_Generic_BuildGetQuery()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
//            var employee = new {FirstName = "Joseph", LastName = "McNeal Jr"};
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder,"Employee",null);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee ");
//        }

//        [Test]
//        public void Test_Generic_BuildGetQuery_WithWhereClause()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
//            var whereClause = $"WHERE FirstName like '%Joseph%'";
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder, "Employee",  whereClause);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee " + whereClause);
//        }


//        [Test]
//        public void Test_Generic_BuildGetQuery_WithWhereClause_And_NullTableName()
//        {
//            var stringBuilder = new StringBuilder();
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
//            var whereClause = $"WHERE FirstName like '%Joseph%'";
//            sqlServerObjectToSql.BuildGetQuery<Employee>(stringBuilder, null, whereClause);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "SELECT * FROM Employee " + whereClause);
//        }




//        [Test]
//        public void Test_Generic_BuildDeleteQuery_With_SQLColumnAttribute_MappedColumn()
//        {
//            var stringBuilder = new StringBuilder();
//            var employee = new EmployeeSqlColumnAttribute() { FirstName = "Joseph", LastName = "McNeal Jr" };
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);

//            sqlServerObjectToSql.BuildDeleteQuery(stringBuilder, nameof(Employee), employee, e => e.FirstName);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "DELETE FROM Employee WHERE [MapColumn]=@FirstName");
//        }


//        [Test]
//        public void Test_Generic_BuildUpdateQuery_With_SQLColumnAttribute_MappedColumn()
//        {
//            var stringBuilder = new StringBuilder();
//            var employee = new EmployeeSqlColumnAttribute() { FirstName = "Joseph", LastName = "McNeal Jr" };
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);

//            sqlServerObjectToSql.BuildUpdateQuery(stringBuilder, nameof(Employee), employee, e => e.FirstName);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "UPDATE Employee SET [MapColumn]=@FirstName,[LastName]=@LastName WHERE [MapColumn]=@FirstName");
//        }


//        [Test]
//        public void Test_Generic_BuildUpdateQuery_With_SQLColumnAttribute_MappedColumn_WithPrimaryKey()
//        {
//            var stringBuilder = new StringBuilder();
//            var employee = new EmployeeSqlColumnAttribute() { FirstName = "Joseph", LastName = "McNeal Jr" };
//            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);

//            sqlServerObjectToSql.BuildUpdateQuery(stringBuilder, nameof(Employee), employee, e => e.FirstName);
//            var sql = stringBuilder.ToString();
//            Assert.AreEqual(sql, "UPDATE Employee SET [MapColumn]=@FirstName,[LastName]=@LastName WHERE [MapColumn]=@FirstName");
//        }


//    }
//}