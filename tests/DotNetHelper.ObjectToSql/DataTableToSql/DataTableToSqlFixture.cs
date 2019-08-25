//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DotNetHelper.ObjectToSql.Enum;
//using NUnit.Framework;

//namespace DotNetHelper.ObjectToSql.Tests.DataTableToSql
//{
//    class DataTableToSqlFixture : BaseTest
//    {

//        public DataTable MockDataPrimaryKey { get; } = GetMockData();

//        private static DataTable GetMockData()
//        {
//            var dt = new DataTable("Employee");
//            dt.Columns.Add("PrimaryKey", typeof(int));
//            dt.Columns.Add("FirstName", typeof(string));
//            dt.Columns.Add("LastName", typeof(string));
//            dt.Rows.Add(1, "Ivan","dsfdsf");
//            dt.PrimaryKey = new[] {dt.Columns["PrimaryKey"]};
//            return dt;
//        }

//        [SetUp]
//        public void Setup()
//        {

//        }
//        [TearDown]
//        public void Teardown()
//        {

//        }

//        [Test]
//        public void Test_Anonymous_T_BuildQuery_With_Specified_Table_Name()
//        {
//            RunTestOnAllDBTypes(delegate(DataBaseType type)
//            {
//                var dt2Sql = new Services.DataTableToSql(type);

//                dt2Sql.BuildQuery(MockData, ActionType.Insert);
                
//            });
            
//        }
//    }
//}
