using System;
using DotNetHelper.ObjectToSql.Enum;
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
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithMappedColumnSqlColumn.ToSql(ActionType, type));
            });
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType, type));
            });
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIgnorePropertySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIgnorePropertySqlColumn.ToSql(ActionType, type));
            });

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType, type));
            });

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);
                var sql = objectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(ActionType);
                Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType, type));
            });

        }



        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);

                var sql = string.Empty;
                if (type == DataBaseType.Sqlite || type == DataBaseType.MySql)
                {
                    EnsureExpectedExceptionIsThrown<NotImplementedException>(() =>
                        objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(ActionType,
                            "Employee", a => a.PrimaryKey)
                    );
                    return;
                }
                else
                {
                    sql = objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(ActionType,
                       "Employee", a => a.PrimaryKey);
                }

                var value = $"";
                switch (type)
                {
                    case DataBaseType.SqlServer:
                        value = "INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) OUTPUT INSERTED.[PrimaryKey] \r\n VALUES (@FirstName,@LastName,@PrimaryKey)";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                Assert.AreEqual(sql, value);
            });
        }





        [Test]
        public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            RunTestOnAllDBTypes(delegate (DataBaseType type)
            {
                var objectToSql = new Services.ObjectToSql(type);

                var sql = string.Empty;
                if (type == DataBaseType.Sqlite || type == DataBaseType.MySql)
                {
                    EnsureExpectedExceptionIsThrown<NotImplementedException>(() =>
                        objectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>(ActionType,
                            "Employee", e => e.FirstName)
                    );
                    return;
                }
                else
                {

                    sql = objectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnSqlColumn>(ActionType,
                       "Employee", e => e.FirstName);
                }

                var expected = "";
                switch (type)
                {
                    case DataBaseType.SqlServer:
                        expected = "INSERT INTO Employee ([FirstName2],[LastName]) OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)";
                        break;
                    case DataBaseType.MySql:
                        break;
                    case DataBaseType.Sqlite:
                        expected = "INSERT INTO Employee ([FirstName2],[LastName]) OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)";
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
                Assert.AreEqual(sql, expected);
            });
        }




    }
}