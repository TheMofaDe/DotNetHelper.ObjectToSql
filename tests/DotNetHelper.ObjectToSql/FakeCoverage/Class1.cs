using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Helper;
using DotNetHelper.ObjectToSql.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.FakeCoverage
{

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [SqlColumn(SerializableType = SerializableType.JSON)]
        public Team Team { get; set; }
    }

    public class Team
    {
        public string TeamName { get; set; }
    }

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

        [Test]
        public void Test() // TODO  :: MAKE LEGIT TEST CASES OUT OF THIS 
        {
            var dbtypes = System.Enum.GetValues(typeof(DataBaseType)).Cast<DataBaseType>().ToList();
            dbtypes.ForEach(delegate (DataBaseType type)
            {
                var syntaxHelper = new SqlSyntaxHelper(type);
                var objToSql = new Services.ObjectToSql(type);


                var employee = new Employee();

                var param = objToSql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o), null, o => JsonConvert.SerializeObject(o), null);
                var tableExistSQL = syntaxHelper.BuildTableExistStatement(new SQLTable(type, "TEST"), "", "");
                var sql = objToSql.BuildQuery<Employee>(ActionType.Insert);

                syntaxHelper.ConvertParameterSqlToReadable(param, sql, Encoding.UTF8);
            });




        }
    }
}
