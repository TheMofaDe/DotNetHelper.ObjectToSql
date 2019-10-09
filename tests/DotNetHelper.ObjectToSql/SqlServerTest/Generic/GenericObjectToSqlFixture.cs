using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using DotNetHelper.ObjectToSql;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Services;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace Tests
{
    public class AnonmyousObjectToSqlFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Build_Parameters_Uses_Runtime_Type()
        {
            var obj2Sql = new ObjectToSql(DataBaseType.SqlServer,true);
            var hashSet = new HashSet<Employee>(new List<Employee>() { new Employee(){FirstName = "joif",LastName = "dsfi"}});
            var data = hashSet.ToList();
            if (data.GetType().IsTypeAnIEnumerable()) 
            {
                if (data is IEnumerable<object> list)
                {
                    foreach (var item in list)
                    {
                        var parameters = obj2Sql.BuildDbParameterList(item,delegate(string s, object o) { return new SqlParameter(s,o); } );
                        Assert.That(parameters != null && parameters.Count == 2);
                    }
                }
            }
        }


    


    }

    public static class Extensions {

    public static bool IsTypeAnIEnumerable(this Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type);
    }

}
}