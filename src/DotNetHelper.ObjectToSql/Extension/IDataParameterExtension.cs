using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DotNetHelper.ObjectToSql.Extension
{
    public static class IDataParameterExtension
    {
        public static void AddRange<T>(this IDataParameterCollection collection, IEnumerable<T> collectionToAdd)
        {
            var toAdd = collectionToAdd as IList<T> ?? collectionToAdd.ToList();
            for (var i = 0; i < toAdd.ToList().Count(); i++)
            {
                collection.Add(toAdd[i]);
            }
        }


        public static string ParamToSql(this IDataParameterCollection list, string query)
        {
            var sql = query.Clone().ToString();
            // Convert Query To Human Readable  
            var temp = new List<DbParameter>() { };
            for (var i = 0; i < list.Count; i++)
            {
                temp.Add(list[i] as DbParameter);
            }
            temp = temp.OrderByDescending(x => x.ParameterName).ToList();
            temp.ForEach(delegate (DbParameter parameter)
            {
                var name = parameter.ParameterName;
                sql = sql.Replace(name.StartsWith("@") ? $"{name}" : $"@{name}", CommandToSQl(parameter.Value, Encoding.UTF8));
            });

            return sql;

        }


        private static string CommandToSQl(object obj, Encoding encoding)
        {

            if (obj == null || obj == DBNull.Value)
            {
                return "NULL";
            }
            else if (obj is byte[])
            {
                var value = (byte[])obj;
                return value.Length <= 0 ? $@"NULL" : $@"'{encoding.GetString(value)}'";
            }
            else if (obj is string)
            {
                var value = obj.ToString();
                return $@"'{value.Replace("'", "''")}'"; // escape single quotes
            }
            else if (obj is int?)
            {
                var value = obj as int?;
                return $@"{value}";
            }
            else if (obj is float?)
            {
                var value = obj as float?;
                return $@"{value}";
            }
            else if (obj is DateTime?)
            {
                var value = obj as DateTime?;
                return value == DateTime.MinValue ? $"NULL" : $@"'{value:s}'";
            }
            else if (obj is bool?)
            {
                var value = obj as bool?;
                return (bool)value ? $"1" : $"0"; //$@"{value}";
            }
            else if (obj is Guid?)
            {
                var value = obj as Guid?;
                return $@"CAST('{value}' AS UNIQUEIDENTIFIER)";

            }
            else
            {
                // We Convert Non System Types To Json
                return $"NULL";

            }

        }


    }
}
