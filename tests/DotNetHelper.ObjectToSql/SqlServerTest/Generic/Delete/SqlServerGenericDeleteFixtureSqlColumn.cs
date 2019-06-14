﻿using System;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Delete
{
    public class SqlServerGenericDeleteFixtureSqlColumn
    {

        public ActionType ActionType { get; } = ActionType.Delete;

        [SetUp]
        public void Setup()
        {

        }
        [TearDown]
        public void Teardown()
        {
     
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Ensure_MissingKeyException_Is_Thrown_()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            Assert.That(() => sqlServerObjectToSql.BuildQuery<EmployeeWithMappedColumnSqlColumn>( nameof(EmployeeWithMappedColumnSqlColumn),ActionType,null),
                Throws.Exception
                    .TypeOf<MissingKeyAttributeException>());
        }


        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Identity_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>( nameof(EmployeeWithIdentityKeySqlColumn),ActionType,null);
            Assert.AreEqual(sql,EmployeeWithIdentityKeySqlColumn.ToSql(ActionType) );
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));
        }

        [Test]
        public void Test_Generic_BuildDeleteQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
        {
            var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>(nameof(EmployeeWithManyPrimaryKeySqlColumn), ActionType, null);
            Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType));
        }


        //[Test]
        //public void Test_Generic_BuildDeleteQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
        //{
        //    var sqlServerObjectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    var sql = sqlServerObjectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(nameof(EmployeeWithPrimaryKeySqlColumn), ActionType, null);
        //    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType));

        //    sqlServerObjectToSql.BuildDeleteQuery<EmployeeWithPrimaryKeySqlColumn>(StringBuilder, nameof(Employee), e => e.FirstName);
        //    Assert.AreEqual(StringBuilder.ToString(), "DELETE FROM Employee WHERE [FirstName]=@FirstName");
        //}



    }
}