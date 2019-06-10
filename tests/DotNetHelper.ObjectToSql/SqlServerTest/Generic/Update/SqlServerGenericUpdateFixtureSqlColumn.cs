using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
    public class SqlServerGenericUpdateFixtureSqlColumn
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
        public void Test_Generic_BuildUpdateQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [FirstName2]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey");
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithIgnorePropertyAndKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey");
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithIdentityKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [IdentityKey]=@IdentityKey");
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey");
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithManyPrimaryKeySqlColumn>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey,[PrimaryKey1]=@PrimaryKey1 WHERE [PrimaryKey]=@PrimaryKey AND [PrimaryKey1]=@PrimaryKey1");
        }


        [Test]
        public void Test_Generic_BuildUpdateQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildUpdateQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee),e => e.FirstName);
            Assert.AreEqual(StringBuilder.ToString(), "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [FirstName]=@FirstName");
        }



    }
}