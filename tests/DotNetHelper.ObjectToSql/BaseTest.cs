using System;
using System.Collections.Generic;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests
{
    public class BaseTest
    {

        public bool SkipTestDueToFalsePositive { get; set; }

        public List<DataBaseType> SupportedDBType { get; } = new List<DataBaseType>()
        {
             DataBaseType.SqlServer
            ,DataBaseType.Sqlite
            ,DataBaseType.MySql
        };

        public void RunTestOnAllDBTypes(Action<DataBaseType> testCase)
        {
            SupportedDBType.ForEach(delegate (DataBaseType type)
            {
                testCase.Invoke(type);
            });
        }


        public void EnsureExpectedExceptionIsThrown<T>(Action action) where T : Exception
        {
            Assert.That(action.Invoke, Throws.Exception.TypeOf<T>());
        }
    }
}
