using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
    public class SqlServerGenericUpdateFixtureDataAnnotation : BaseTest
    {

        public ActionType ActionType { get; } = ActionType.Update;

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql =
                    sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertyAndKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIgnorePropertyAndKeyDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType, type));

            });

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var sqlServerObjectToSql = new Services.ObjectToSql(type);
                var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithManyPrimaryKeyDataAnnotation.ToSql(ActionType, type));
            });
        }


    }
}