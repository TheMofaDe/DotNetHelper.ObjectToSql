using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixture : BaseTest
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
        public void Test_Generic_Build_Insert_Query_With_Outputs()
        {

            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                if (type == DataBaseType.Sqlite || type == DataBaseType.MySql) return;
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQueryWithOutputs<Employee>(ActionType, e => e.FirstName);

                var expected = "";

                switch (type)
                {
                    case DataBaseType.SqlServer:
                        expected = "INSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName)";
                        break;
                    case DataBaseType.MySql:
                        break;
                    case DataBaseType.Sqlite:
                        expected = "NOT SUPPORTED";
                        break;
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        break;
                    case DataBaseType.Odbc:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                Assert.AreEqual(sql, expected);
            });
        }



        [Test]
        public void Test_Generic_Build_Insert_Query()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
                {
                    var objectToSql = new Services.ObjectToSql(type);
                    var sql = objectToSql.BuildQuery(ActionType, new Employee());
                    Assert.AreEqual(sql, Employee.ToSql(ActionType, type));
                });
        }

        [Test]
        public void Test_Generic_As_Object_Build_Insert_Query()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
                {
                    object employee = new Employee();
                    var objectToSql = new Services.ObjectToSql(type);
                    var sql = objectToSql.BuildQuery(ActionType, employee);
                    Assert.AreEqual(sql, Employee.ToSql(ActionType, type));
                });
        }

        [Test]
        public void Test_Generic_Build_Insert_Query_Uses_Type_Name_When_Table_Name_Is_Not_Specified()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<Employee>(ActionType);
                Assert.AreEqual(sql, Employee.ToSql(ActionType, type));
            });
        }





        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs()
        {
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = objectToSql.BuildQueryWithOutputs<Employee>(ActionType, "Employee", e => e.FirstName);
            Assert.AreEqual(sql, $"INSERT INTO Employee ([FirstName],[LastName]) {Environment.NewLine} OUTPUT INSERTED.[FirstName] {Environment.NewLine} VALUES (@FirstName,@LastName)");
        }







        [Test]
        public void Test_BuildDbParameterList_Contains_Accurate_Values()
        {
            var employee = new Employee() { LastName = "John", FirstName = "Doe" };
            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

            var parameters = objectToSql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o), null, null, null);

            Assert.AreEqual(parameters.First().Value, "Doe");
            Assert.AreEqual(parameters.Last().Value, "John");
            Assert.AreEqual(parameters.Count, 2);

        }




    }
}