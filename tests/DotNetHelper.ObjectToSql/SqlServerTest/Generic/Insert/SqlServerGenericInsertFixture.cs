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

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixture
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
        public void Test_Generic_Build_Insert_Query_Throws_ArgumentNull_When_RunTimeAttribute_Mapping_IsNull()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildQuery<Employee>("Employee", ActionType, new Employee(), null),
                        Throws.Exception
                            .TypeOf<ArgumentNullException>());
        }


        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQueryWithOutputs<Employee>(nameof(Employee),ActionType, e => e.FirstName);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName)");
        }




        [Test]
        public void Test_SqlTable()
        {
          
            var sqlTable = new SQLTable(DataBaseType.SqlServer,typeof(Employee));
            Assert.AreEqual(sqlTable.TableName,"Employee","Table Name is not what was expected");
        }


        [Test]
        public void Test_SqlTable_FullNameWithBrackets()
        {

            var sqlTable = new SQLTable(DataBaseType.SqlServer, typeof(Employee));
            Assert.AreEqual(sqlTable.FullNameWithBrackets, "[Employee]", "Table Name is not what was expected");
        }


        [Test]
        public void Test_SqlTable_TableName_Doesnt_Include_Brackets()
        {

            var sqlTable = new SQLTable(DataBaseType.SqlServer, "[Employee]");
            Assert.AreEqual(sqlTable.TableName, "Employee", "Table Name is not what was expected");
        }



        [Test]
        public void Test_BuildDbParameterList_Contains_Accurate_Values()
        {
            var employee = new Employee() {LastName = "John", FirstName = "Doe"};
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

            var parameters = sqlServerObjectToSql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o), null, null, null);

            Assert.AreEqual(parameters.First().Value,"Doe");
            Assert.AreEqual(parameters.Last().Value, "John");
            Assert.AreEqual(parameters.Count, 2);

        }




    }
}