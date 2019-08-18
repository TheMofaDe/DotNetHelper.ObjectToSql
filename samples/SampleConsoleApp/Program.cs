using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
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

            Console.WriteLine(HowToUseWithDynamicObjects());
            Console.ReadLine();

        }


        #region README.MD


        // HOW TO GENERATE SQL 
        private static void HowToUseWithGenericTypesDetailed()
        {
            // ## HOW TO USE WITH GENERICS TYPES
            var actionType = ActionType.Insert; // A enum with the values Insert,Update,Delete,Upsert
            var databaseType = DataBaseType.SqlServer; // A enum with values of SQLServer,SQLite & more
            var obj2Sql = new ObjectToSql(databaseType);
            var insertSql = obj2Sql.BuildQuery<Employee>(actionType);
            // Additional Overload Methods
            var insertSql1 = obj2Sql.BuildQuery<Employee>(actionType, "TableName");
            var insertSql2 = obj2Sql.BuildQuery(actionType, new Employee());
            var insertSql3 = obj2Sql.BuildQuery(actionType, new Employee(), "TableName");
        }
        private static string HowToUseWithGenericTypes()
        {
            var insertSql = new ObjectToSql(DataBaseType.SqlServer).BuildQuery<Employee>(ActionType.Insert);
            return insertSql;
        }
        private static string HowToUseWithGenericObjects()
        {
            var insertSql = new ObjectToSql(DataBaseType.SqlServer).BuildQuery(ActionType.Insert, new Employee());
            return insertSql;
        }
        private static string HowToUseWithDynamicObjects()
        {
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";
            var insertSql = new ObjectToSql(DataBaseType.SqlServer).BuildQuery(ActionType.Insert, record, "Employee");
            return insertSql;
        }
        private static string HowToUseWithAnonymousObjects()
        {
            var obj = new { FirstName = "John", LastName = "Doe" };
            var insertSql = new ObjectToSql(DataBaseType.SqlServer).BuildQuery(ActionType.Insert, obj, "Employee");
            return insertSql;
        }
        // HOW Tstatic O GENERATE DBParameters
        private static void HowToGenerateDBParameters()
        {
            var obj2Sql = new ObjectToSql(DataBaseType.SqlServer);
            List<DbParameter> dbParameters = obj2Sql.BuildDbParameterList(new Employee(), (s, o) => new SqlParameter(s, o));
        }
        private static void HowToGenerateSqlFromDataTable()
        {
            var deleteSql = new DataTableToSql(DataBaseType.SqlServer).BuildQuery(new DataTable("Employee"), ActionType.Delete);
        }

        #endregion
    }
}
