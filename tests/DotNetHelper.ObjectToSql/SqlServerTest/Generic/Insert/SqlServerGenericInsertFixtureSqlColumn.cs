using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixtureSqlColumn
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
        public void Test_Generic_BuildInsertQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery<EmployeeWithMappedColumnSqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName2],[LastName]) VALUES (@FirstName,@LastName)");
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery<EmployeeWithIgnorePropertySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([LastName]) VALUES (@LastName)");
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery<EmployeeWithIdentityKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey)");
        }

        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs_Ensure_MissingIdenityKey_Is_Thrown()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee)),
                Throws.Exception
                    .TypeOf<EmptyArgumentException>());
        }



        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>(StringBuilder, nameof(Employee), e => e.FirstName);
            Assert.AreEqual(StringBuilder.ToString(), "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        }



    }
}