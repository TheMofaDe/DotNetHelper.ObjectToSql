using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerGenericInsertFixtureDataAnnotation : BaseTest
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
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithMappedColumnDataAnnotation.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType, type));
            });
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIgnorePropertyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIgnorePropertyDataAnnotation.ToSql(ActionType, type));
            });

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType, type));
            });

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(ActionType);
                Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType, type));
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
                        objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(
                            ActionType, "Employee", a => a.PrimaryKey)
                        );
                    return;
                }
                else
                {

                    sql = objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(
                       ActionType, "Employee", a => a.PrimaryKey);
                }

                var answer = string.Empty;
                switch (type)
                {
                    case DataBaseType.SqlServer:
                        answer = $"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) {Environment.NewLine} OUTPUT INSERTED.[PrimaryKey] {Environment.NewLine} VALUES (@FirstName,@LastName,@PrimaryKey)";
                        break;
                    case DataBaseType.MySql:
                        break;
                    case DataBaseType.Sqlite:
                        answer = $"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) {Environment.NewLine} OUTPUT INSERTED.[PrimaryKey] {Environment.NewLine} VALUES (@FirstName,@LastName,@PrimaryKey)";
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
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithTableAttribute>(ActionType);
                Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType, type));
            });
        }



        [Test]
        public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = string.Empty;
                if (type == DataBaseType.Sqlite)
                {
                    EnsureExpectedExceptionIsThrown<NotImplementedException>(() =>
                        objectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnDataAnnotation>(ActionType,
                            "Employee", e => e.FirstName)
                    );
                    return;
                }
                else
                {

                    sql = objectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnDataAnnotation>(ActionType,
                        "Employee", e => e.FirstName);
                }

                Assert.AreEqual(sql, $"INSERT INTO Employee ([FirstName2],[LastName]) {Environment.NewLine} OUTPUT INSERTED.[FirstName2] {Environment.NewLine} VALUES (@FirstName,@LastName)");
            });
        }






    }
}