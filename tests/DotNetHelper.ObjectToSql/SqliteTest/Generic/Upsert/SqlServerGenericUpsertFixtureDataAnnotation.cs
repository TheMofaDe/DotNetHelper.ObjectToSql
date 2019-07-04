using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Upsert
{
    public class SqliteGenericUpsertFixtureDataAnnotation
    {


        public ActionType ActionType { get; } = ActionType.Upsert;

        [SetUp]
        public void Setup()
        {
            
        }
        [TearDown]
        public void Teardown()
        {
            
        }


        [Test]
        public void Test_Generic_BuildUpsertQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>( nameof(Employee),ActionType.Upsert);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) WHERE [PrimaryKey]=@PrimaryKey ON CONFLICT DO UPDATE SET [FirstName2]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey");
        }


        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(nameof(EmployeeWithIdentityKeySqlColumn), ActionType.Upsert, column => column.FirstName);
            Assert.AreEqual(sql, "IF EXISTS ( SELECT * FROM EmployeeWithIdentityKeySqlColumn WHERE [FirstName]=@FirstName ) BEGIN UPDATE EmployeeWithIdentityKeySqlColumn SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [FirstName]=@FirstName END ELSE BEGIN INSERT INTO EmployeeWithIdentityKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName) END");
        }


     

        //[Test]
        //public void Test_Generic_BuildUpsertQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
        //    SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>( nameof(Employee),e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "IF EXISTS ( SELECT * FROM Employee WHERE [FirstName]=@FirstName ) " +
        //                                              "BEGIN " +
        //                                              "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [FirstName]=@FirstName " +
        //                                              "END ELSE BEGIN " +
        //                                              "INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) " +
        //                                              "END");
        //}



    }
}