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
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType));

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType));

        }

        //[Test]
        //public void Test_Generic_BuildInsertQueryWithOutputs_Ensure_Missing_Identity_Key_Is_Thrown()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    Assert.That(() => sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(new StringBuilder(), nameof(Employee)),
        //        Throws.Exception
        //            .TypeOf<EmptyArgumentException>());
        //}





        //[Test]
        //public void Test_Generic_BuildInsertQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        //{
        //    var stringBuilder = new StringBuilder();
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);

        //    sqlServerObjectToSql.BuildInsertQueryWithOutputs<EmployeeWithMappedColumnDataAnnotation>(stringBuilder, nameof(Employee), e => e.FirstName);
        //    var sql = stringBuilder.ToString();
        //    Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        //}






    }
}