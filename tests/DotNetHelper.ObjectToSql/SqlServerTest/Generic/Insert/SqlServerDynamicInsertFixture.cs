using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerDynamicInsertFixtureDataAnnotation
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
        public void Test_Dynamic_BuildInsertQuery()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";
            var sql =sqlServerObjectToSql.BuildQuery("Employee",ActionType.Insert,record);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }


        [Test]
        public void Test_Dynamic_BuildUpdateQuery_Ensure_Exception_MissingKeyAttributeException_Thrown_When_NoKeys_Provided()
        {
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";

            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildQuery(nameof(Employee), ActionType.Update, record),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());


        }

        [Test]
        public void Test_Dynamic_BuildUpdateQuery()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject(); 
            record.LastName = "Doe";
            record.PrimaryKey = 1;

            var attribute = new RunTimeAttributeMap("PrimaryKey", new List<System.Attribute>() {new KeyAttribute()});
            var list = new List<RunTimeAttributeMap>()
            {
                attribute
            };

            var sql = sqlServerObjectToSql.BuildQuery("Employee",ActionType.Insert,record,list);
            Assert.AreEqual(sql, "UPDATE Employee SET [LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey");
        }


        //[Test]
        //public void Test_Dynamic_BuildInsertQueryWithOutputs_Ensure_MissingIdenityKey_Is_Thrown()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    Assert.That(() => sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee)),
        //        Throws.Exception
        //            .TypeOf<EmptyArgumentException>());
        //}








    }
}