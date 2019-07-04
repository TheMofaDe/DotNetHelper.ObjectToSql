﻿using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqliteTest.Generic.Insert
{
    public class SqliteGenericInsertFixtureDataAnnotation
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
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Uses_Mapped_Column_Name_Instead_Of_PropertyName_Insert_Key()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Include_Ignored_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIgnorePropertyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildInsertQuery_Doesnt_Try_To_Insert_Identity_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType));

        }


        [Test]
        public void Test_Generic_BuildInsertQuery_Does_Try_To_Insert_PrimaryKey_Column()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildQueryWithOutputs()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeyDataAnnotation>(nameof(Employee),
                ActionType, a => a.PrimaryKey);
            Assert.AreEqual(sql, $@"INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) 
 OUTPUT INSERTED.[PrimaryKey] 
 VALUES (@FirstName,@LastName,@PrimaryKey)");
        }

        [Test]
        public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);
            var sql = SqliteObjectToSql.BuildQuery<EmployeeWithTableAttribute>(null, ActionType);
            Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType));
        }



        [Test]
        public void Test_Generic_BuildQueryWithOutputs_Uses_MappedColumn_Name_Instead_Of_PropertyName()
        {
            var SqliteObjectToSql = new Services.ObjectToSql(DataBaseType.Sqlite);

            var sql = SqliteObjectToSql.BuildQueryWithOutputs<EmployeeWithMappedColumnDataAnnotation>(nameof(Employee),ActionType, e => e.FirstName);
            Assert.AreEqual(sql, "INSERT INTO Employee ([FirstName2],[LastName]) \r\n OUTPUT INSERTED.[FirstName2] \r\n VALUES (@FirstName,@LastName)");
        }






    }
}