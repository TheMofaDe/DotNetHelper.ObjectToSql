using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Upsert
{
    public class SqlServerGenericUpsertFixtureDataAnnotation : BaseTest
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


        //[Test]
        //public void Test_Generic_BuildUpsertQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        //{
        //    RunTestOnAllDBTypes(delegate (DataBaseType type) {
        //        var objectToSql = new Services.ObjectToSql(type);
        //    var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(ActionType);
        //    Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType,type));
        //    });
        //}


        [Test]
        public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType, null, column => column.FirstName);


                var answer = "";
                switch (type)
                {
                    case DataBaseType.SqlServer:
                        answer =
                            "IF EXISTS ( SELECT TOP 1 * FROM EmployeeWithIdentityKeySqlColumn WHERE [FirstName]=@FirstName ) BEGIN UPDATE EmployeeWithIdentityKeySqlColumn SET [LastName]=@LastName WHERE [FirstName]=@FirstName END ELSE BEGIN INSERT INTO EmployeeWithIdentityKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName) END";
                        break;
                    case DataBaseType.MySql:
                        break;
                    case DataBaseType.Sqlite:
                        answer = "INSERT INTO EmployeeWithIdentityKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName) ON CONFLICT ([FirstName] DO UPDATE SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [FirstName]=@FirstName";
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
                Assert.AreEqual(sql, answer);

            });
        }



        [Test]
        public void Test_Generic_BuildQuery()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery(ActionType, new EmployeeWithIdentityKeySqlColumn());

                Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType, type));

            });
        }




        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);

                var sql = string.Empty;
                if (type == DataBaseType.Sqlite)
                {
                    EnsureExpectedExceptionIsThrown<NotImplementedException>(() =>
                        objectToSql.BuildQueryWithOutputs<EmployeeWithIdentityKeySqlColumn>(ActionType,
                            "Employee", a => a.FirstName)
                    );
                    return;
                }
                else
                {

                    sql = objectToSql.BuildQueryWithOutputs<EmployeeWithIdentityKeySqlColumn>(
                        ActionType, nameof(Employee), a => a.FirstName);
                }

                Assert.AreEqual(sql, "IF EXISTS ( SELECT * FROM Employee WHERE [IdentityKey]=@IdentityKey ) BEGIN UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName OUTPUT DELETED.[FirstName]  WHERE [IdentityKey]=@IdentityKeyINSERT INTO Employee ([FirstName],[LastName]) \r\n OUTPUT INSERTED.[FirstName] \r\n VALUES (@FirstName,@LastName) END ELSE BEGIN  END");
            });
        }



        //[Test]
        //public void Test_Generic_BuildUpsertQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    objectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "IF EXISTS ( SELECT * FROM Employee WHERE [FirstName]=@FirstName ) " +
        //                                              "BEGIN " +
        //                                              "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName,[PrimaryKey]=@PrimaryKey WHERE [FirstName]=@FirstName " +
        //                                              "END ELSE BEGIN " +
        //                                              "INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) " +
        //                                              "END");
        //}



    }
}