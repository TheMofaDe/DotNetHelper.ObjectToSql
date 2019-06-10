using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerDynamicInsertFixtureDataAnnotation
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
        public void Test_Dynamic_BuildInsertQuery()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";
            sqlServerObjectToSql.BuildInsertQuery(record,StringBuilder,"Employee");
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }


        [Test]
        public void Test_Dynamic_BuildUpdateQuery_Ensure_Exception_Thrown_When_NoKeys_Provided()
        {
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";

            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildUpdateQuery(record, new StringBuilder(), "Employee"),
                Throws.Exception
                    .TypeOf<EmptyArgumentException>());
        }

        [Test]
        public void Test_Dynamic_BuildUpdateQuery()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject(); 
            record.LastName = "Doe";
            record.PrimaryKey = 1;
            sqlServerObjectToSql.BuildUpdateQuery(record, StringBuilder, "Employee",new List<string>(){ "PrimaryKey" });
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey");
        }


        [Test]
        public void Test_Dynamic_BuildInsertQueryWithOutputs_Ensure_MissingIdenityKey_Is_Thrown()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee)),
                Throws.Exception
                    .TypeOf<EmptyArgumentException>());
        }








    }
}