using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Insert
{
    public class SqliteGenericInsertFixture
    {

     
        public ActionType ActionType { get; } = ActionType.Insert;
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
        public void Test_Generic_Build_Insert_Query()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery("Employee", ActionType,new Employee());
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_As_Object_Build_Insert_Query()
        {
            object employee = new Employee();
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery("Employee", ActionType,employee);
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_Build_Insert_Query_Uses_Type_Name_When_Table_Name_Is_Not_Specified()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<Employee>(null, ActionType);
            Assert.AreEqual(sql, Employee.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_Build_Insert_Query_Throws_ArgumentNull_When_RunTimeAttribute_Mapping_IsNull()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            Assert.That(() => SqliteObjectToSql.BuildQuery<Employee>("Employee", ActionType, new Employee(), null),
                        Throws.Exception
                            .TypeOf<ArgumentNullException>());
        }


        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQueryWithOutputs<Employee>(nameof(Employee),ActionType, e => e.FirstName);
            Assert.AreEqual(sql, $"INSERT INTO Employee ([FirstName],[LastName]) {Environment.NewLine} OUTPUT INSERTED.[FirstName] {Environment.NewLine} VALUES (@FirstName,@LastName)");
        }




     


        [Test]
        public void Test_BuildDbParameterList_Contains_Accurate_Values()
        {
            var employee = new Employee() {LastName = "John", FirstName = "Doe"};
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);

            var parameters = SqliteObjectToSql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o), null, null, null);

            Assert.AreEqual(parameters.First().Value,"Doe");
            Assert.AreEqual(parameters.Last().Value, "John");
            Assert.AreEqual(parameters.Count, 2);

        }




    }
}