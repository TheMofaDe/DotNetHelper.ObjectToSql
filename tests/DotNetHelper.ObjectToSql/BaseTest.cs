using System;
using System.Collections.Generic;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests
{
    public class BaseTest
    {

        public List<DataBaseType> SupportedDBType { get; } = new List<DataBaseType>()
        {
             DataBaseType.SqlServer
            ,DataBaseType.Sqlite
        };

        public void RunTestOnAllDBTypes(Action<DataBaseType> testCase)
        {
            SupportedDBType.ForEach(delegate (DataBaseType type)
            {
                testCase.Invoke(type);
            });
        }
    }
}
