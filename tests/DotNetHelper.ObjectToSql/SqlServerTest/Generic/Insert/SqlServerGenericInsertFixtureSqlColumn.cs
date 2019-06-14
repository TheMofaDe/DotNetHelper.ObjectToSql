using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixtureSqlColumn
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
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>(null,ActionType,new EmployeeWithMappedColumnSqlColumn());
            Assert.AreEqual(sql, EmployeeWithMappedColumnSqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(null, ActionType, new EmployeeWithMappedColumnAndPrimaryKeySqlColumn());
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertySqlColumn>(null, ActionType, new EmployeeWithIgnorePropertySqlColumn());
            Assert.AreEqual(sql, EmployeeWithIgnorePropertySqlColumn.ToSql(ActionType));
            
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(null, ActionType, new EmployeeWithIdentityKeySqlColumn());
            Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType));

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(null, ActionType, new EmployeeWithPrimaryKeySqlColumn());
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildInsertQueryWithOutputs_Ensure_Missing_Identity_Key_Is_Thrown()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(new StringBuilder(), nameof(Employee)),
                Throws.Exception
                    .TypeOf<EmptyArgumentException>());
        }



        //[Test]
        //public void Test_Generic_BuildInsertQueryWithOutputs_Uses_Mapped_Column_Name_Instead_Of_PropertyName()
        //{
        //    var sb = new StringBuilder();
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>(sb, nameof(Employee), e => e.FirstName);
        //    Assert.AreEqual(sb.ToString(), "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        //}



    }
}