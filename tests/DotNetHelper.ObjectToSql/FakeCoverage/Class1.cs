using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.FakeCoverage
{
    public class ComebackAndEnchanceTestFixture
    {


   

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void Teardown()
        {

        }


        [Test]
        public void Test_Ensure_ObjectToSql_Can_Be_Initialize_Without_Error()
        {
            Services.ObjectToSql obj;
            obj = new Services.ObjectToSql(DataBaseType.SqlServer);
            obj = new Services.ObjectToSql(DataBaseType.Access95);
            obj = new Services.ObjectToSql(DataBaseType.MySql);
            obj = new Services.ObjectToSql(DataBaseType.Odbc);
            obj = new Services.ObjectToSql(DataBaseType.Sqlite);
            obj = new Services.ObjectToSql(DataBaseType.Oracle);
        }
    }
}
