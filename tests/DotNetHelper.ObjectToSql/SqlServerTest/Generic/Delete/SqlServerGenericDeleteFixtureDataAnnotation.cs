using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Delete
{
    public class SqlServerGenericDeleteFixtureDataAnnotation
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
        public void Test_Generic_BuildDeleteQuery_Ensure_MissingKeyException_Is_Thrown_()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithMappedColumnDataAnnotation>(StringBuilder, nameof(Employee)),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithIdentityKeyDataAnnotation>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [IdentityKey]=@IdentityKey");
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [PrimaryKey]=@PrimaryKey");
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithManyPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee));
            Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [PrimaryKey]=@PrimaryKey AND [PrimaryKey1]=@PrimaryKey1");
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeyDataAnnotation>(StringBuilder, nameof(Employee), e => e.FirstName);
            Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [FirstName]=@FirstName");
        }



    }
}