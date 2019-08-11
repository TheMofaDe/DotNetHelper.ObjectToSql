using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixtureDataAnnotation
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
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnDataAnnotation>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertyDataAnnotation>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType));

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(
                ActionType, "Employee", a => a.PrimaryKey);
            Assert.AreEqual(sql, $@"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) 
 OUTPUT INSERTED.[PrimaryKey] 
 VALUES (@FirstName,@LastName,@PrimaryKey)");
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithTableAttribute>(  ActionType);
            Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

            var sql = sqlServerObjectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnDataAnnotation>(ActionType, "Employee", e => e.FirstName);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        }






    }
}