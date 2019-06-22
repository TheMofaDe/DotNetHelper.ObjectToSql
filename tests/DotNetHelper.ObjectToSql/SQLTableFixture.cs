using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Model;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests
{
    public class SQLTableFixture
    {
        


        [Test]
        public void Test_SqlTable()
        {
            var sqlTable = new SQLTable(DataBaseType.SqlServer, "TableName");
            Assert.AreEqual(sqlTable.TableName, "TableName");
            Assert.AreEqual(sqlTable.FullNameWithBrackets, "[TableName]");
            Assert.AreEqual(sqlTable.FullNameWithOutBrackets, "TableName");
        }



        [Test]
        public void Test_SqlTable_With_Schema()
        {
            var sqlTable = new SQLTable(DataBaseType.SqlServer, "Schema.TableName");
            Assert.AreEqual(sqlTable.TableName, "TableName");
            Assert.AreEqual(sqlTable.SchemaName, "Schema" );
            Assert.AreEqual(sqlTable.FullNameWithBrackets, "[Schema].[TableName]");
            Assert.AreEqual(sqlTable.FullNameWithOutBrackets, "Schema.TableName");
        }


        [Test]
        public void Test_SqlTable_With_Schema_And_Database()
        {
            var sqlTable = new SQLTable(DataBaseType.SqlServer, "Database.Schema.TableName");
            Assert.AreEqual(sqlTable.TableName, "TableName");
            Assert.AreEqual(sqlTable.SchemaName, "Schema");
            Assert.AreEqual(sqlTable.DatabaseName, "Database");
            Assert.AreEqual(sqlTable.FullNameWithBrackets, "[Database].[Schema].[TableName]");
            Assert.AreEqual(sqlTable.FullNameWithOutBrackets, "Database.Schema.TableName");
        }

    


    }
}
