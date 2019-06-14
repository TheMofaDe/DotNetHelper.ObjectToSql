﻿using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
    public class SqlServerGenericUpdateFixtureDataAnnotation
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
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation>(nameof(EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIgnorePropertyAndKeyDataAnnotation>(nameof(EmployeeWithIgnorePropertyAndKeyDataAnnotation), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithIgnorePropertyAndKeyDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeyDataAnnotation>(nameof(EmployeeWithIdentityKeyDataAnnotation), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithIdentityKeyDataAnnotation.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeyDataAnnotation>(nameof(EmployeeWithPrimaryKeyDataAnnotation), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeyDataAnnotation.ToSql(ActionType));

        }

        [Test]
        public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeyDataAnnotation>(nameof(EmployeeWithManyPrimaryKeyDataAnnotation), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithManyPrimaryKeyDataAnnotation.ToSql(ActionType));
        }


    }
}