﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests
{
    public  class BaseTest
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
