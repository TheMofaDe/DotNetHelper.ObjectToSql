using System;
using System.Data.SqlClient;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Services;

namespace SampleConsoleApp
{

    public class Employee
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var actionType = ActionType.Update; // A enum with the values Insert,Update,Delete,Upsert
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var updateSql = sqlServerObjectToSql.BuildQuery<Employee>(null, actionType);
            var upsertSql = sqlServerObjectToSql.BuildQuery<Employee>("Employee", ActionType.Upsert);
            var deleteSql = sqlServerObjectToSql.BuildQuery<Employee>("TableName", ActionType.Delete);

            Console.WriteLine(updateSql);
            Console.WriteLine(upsertSql);
            Console.WriteLine(deleteSql); Console.WriteLine("Hello World!");



            var parameters = sqlServerObjectToSql.BuildDbParameterList(new Employee(), (s, o) => new SqlParameter(s, o),null,null,null);
            Console.ReadLine();


            

        }
    }
}
