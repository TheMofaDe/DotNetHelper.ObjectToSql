using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixtureSqlColumn : BaseTest
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
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type) {
                var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>(ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnSqlColumn.ToSql(ActionType,type));
            });
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type) {
                var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType,type));
            });
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type) {
                var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertySqlColumn>(ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertySqlColumn.ToSql(ActionType,type));
            });

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type) {
                    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType,type));
            });

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type) {
                        var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType,type));
            });

        }



 //       [Test]
 //       public void Test_Generic_BuildQueryWithOutputs()
 //       {
 //           RunTestOnAllDBTypes(delegate (DataBaseType type) {
 //                           var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
 //           var sql = sqlServerObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(
 //               ActionType, "Employee", a => a.PrimaryKey);
 //           Assert.AreEqual(sql, $@"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) 
 //OUTPUT INSERTED.[PrimaryKey] 
 //VALUES (@FirstName,@LastName,@PrimaryKey)");
 //           });
 //       }





 //       [Test]
 //       public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
 //       {
 //           RunTestOnAllDBTypes(delegate (DataBaseType type) {
 //                               var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

 //           var sql = sqlServerObjectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>( ActionType, "Employee", e => e.FirstName);
 //           Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
 //           });
 //       }




    }
}