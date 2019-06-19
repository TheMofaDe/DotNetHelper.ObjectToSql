using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixture
    {

     
        public ActionType ActionType { get; } = ActionType.Insert;
        
        [SetUp]
        public void Setup()
        {
          
        }
        [TearDown]
        public void Teardown()
        {
          
        }


        [Test]
        public void Test_Generic_Build_Insert_Query()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery("Employee", ActionType,new Employee());
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_As_Object_Build_Insert_Query()
        {
            object employee = new Employee();
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery("Employee", ActionType,employee);
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_Build_Insert_Query_Uses_Type_Name_When_Table_Name_Is_Not_Specified()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<Employee>(null, ActionType);
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_Build_Insert_Query_When_Object_Instance_Is_Null()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<Employee>("Employee", ActionType);
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }



        //[Test]
        //public void Test_Generic_BuildInsertQueryWithOutputs()
        //{
        //    var stringBuilder = new StringBuilder();
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

        //    sqlServerObjectToSql.BuildInsertQueryWithOutputs<Employee>(stringBuilder, nameof(Employee),e =>e.FirstName  );
        //    var sql = stringBuilder.ToString();
        //    Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName)");
        //}












    }
}