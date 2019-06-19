using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Upsert
{
    public class SqlServerGenericUpsertFixtureDataAnnotation
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
        public void Test_Generic_BuildUpsertQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>( nameof(Employee),ActionType.Upsert);
            Assert.AreEqual(sql, "IF EXISTS ( SELECT * FROM Employee WHERE [PrimaryKey]=@PrimaryKey ) " +
                                                      "BEGIN " +
                                                      "UPDATE Employee SET [FirstName2]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [PrimaryKey]=@PrimaryKey " +
                                                      "END ELSE BEGIN " +
                                                      "INSERT INTO Employee ([FirstName2],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) " +
                                                      "END");
        }

   
        //[Test]
        //public void Test_Generic_BuildUpsertQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>( nameof(Employee),e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "IF EXISTS ( SELECT * FROM Employee WHERE [FirstName]=@FirstName ) " +
        //                                              "BEGIN " +
        //                                              "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [FirstName]=@FirstName " +
        //                                              "END ELSE BEGIN " +
        //                                              "INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) " +
        //                                              "END");
        //}



    }
}