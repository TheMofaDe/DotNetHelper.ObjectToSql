using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Extension
{
    public static class DBConnectionExtension
    {

        public static Services.ObjectToSql ObjToSql<T>(this T dbConnection, bool includeNonPublicProperties = true) where T : DbConnection
        {
            if (typeof(T).Name == "SqlConnection")
            {
                return new Services.ObjectToSql(DataBaseType.SqlServer, includeNonPublicProperties);
            }
            if (typeof(T).Name == "SqliteConnection")
            {
                return new Services.ObjectToSql(DataBaseType.Sqlite, includeNonPublicProperties);
            }
            if (typeof(T).Name == "MySqlConnection")
            {
                return new Services.ObjectToSql(DataBaseType.MySql, includeNonPublicProperties);
            }
            if (typeof(T).Name == "OracleConnection")
            {
                return new Services.ObjectToSql(DataBaseType.Oracle, includeNonPublicProperties);
            }
            return new Services.ObjectToSql(DataBaseType.SqlServer, includeNonPublicProperties);
        }
    }
}
