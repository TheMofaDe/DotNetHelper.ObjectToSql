using System;
using System.Dynamic;
using System.Linq;
using DotNetHelper.ObjectToSql.Enum;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Insert
{
    public class SqlServerDynamicInsertFixture : BaseTest
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
        public void Test_BuildQuery_Throws_InvalidOperation_ForNonInsert_Actions()
        {

            var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
            dynamic obj = new ExpandoObject();
            obj.FirstName2 = "John";
            obj.LastName = "Doe";

            var list = System.Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
            list.ForEach(delegate (ActionType type)
            {
                if (type == ActionType.Insert)
                {
                    Assert.DoesNotThrow(() => objectToSql.BuildQuery(type, obj));
                    return;
                }
                Assert.That(() => objectToSql.BuildQuery(type, obj),
                    Throws.Exception
                        .TypeOf<InvalidOperationException>());
            });


        }

        //[Test]
        //public void Test_BuildQuery_With_KeyRunTime_Attribute()
        //{
        //    var objectToSql = new Services.ObjectToSql(DataBaseType.SqlServer);
        //    dynamic obj = new ExpandoObject();
        //    obj.FirstName2 = "John";
        //    obj.LastName = "Doe";
        //    obj.PrimaryKey = 2;
        //    var attributes = new List<RunTimeAttributeMap>()
        //    {
        //        new RunTimeAttributeMap("PrimaryKey", new List<System.Attribute>() {new KeyAttribute()})
        //    };

        //    var list = System.Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
        //    list.ForEach(delegate (ActionType type)
        //    {
        //        var sql = string.Empty;
        //        switch (type)
        //        {
        //            case ActionType.Insert:
        //                sql = objectToSql.BuildQuery( type, obj,attributes);
        //                Assert.AreEqual(sql, @"INSERT INTO EmployeeWithMappedColumnAndPrimaryKeySqlColumn ([FirstName2],[LastName],[PrimaryKey]) VALUES (@FirstName2,@LastName,@PrimaryKey)");
        //                break;
        //            case ActionType.Update:
        //                sql = objectToSql.BuildQuery("EmployeeWithMappedColumnAndPrimaryKeySqlColumn", type, obj, attributes);
        //                Assert.AreEqual(sql, $@"UPDATE EmployeeWithMappedColumnAndPrimaryKeySqlColumn SET [FirstName2]=@FirstName2,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey");
        //                break;
        //            case ActionType.Upsert:
        //                sql = objectToSql.BuildQuery("EmployeeWithMappedColumnAndPrimaryKeySqlColumn", type, obj, attributes);
        //                Assert.AreEqual(sql, $@"IF EXISTS ( SELECT * FROM EmployeeWithMappedColumnAndPrimaryKeySqlColumn WHERE [PrimaryKey]=@PrimaryKey ) BEGIN UPDATE EmployeeWithMappedColumnAndPrimaryKeySqlColumn SET [FirstName2]=@FirstName2,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey END ELSE BEGIN INSERT INTO EmployeeWithMappedColumnAndPrimaryKeySqlColumn ([FirstName2],[LastName],[PrimaryKey]) VALUES (@FirstName2,@LastName,@PrimaryKey) END");
        //                break;
        //            case ActionType.Delete:
        //                sql = objectToSql.BuildQuery("EmployeeWithMappedColumnAndPrimaryKeySqlColumn", type, obj, attributes);
        //                Assert.AreEqual(sql, $@"DELETE FROM EmployeeWithMappedColumnAndPrimaryKeySqlColumn WHERE [PrimaryKey]=@PrimaryKey");
        //                break;

        //        }
        //    });

        //}





    }
}