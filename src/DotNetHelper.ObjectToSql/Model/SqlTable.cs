using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Extension;

namespace DotNetHelper.ObjectToSql.Model
{
    public class SQLTable
    {
        public string TableName { get; internal set; } 
        public string SchemaName { get; internal set; } 
        public string DatabaseName { get; internal set; }
        public string FullName => GetFullName();


        public SQLTable(Type type)
        {
            while (type.IsTypeAnIEnumerable())
            {
                type = type.GetEnumerableItemType();
            }

            var sqlTableAttribute = type.GetCustomAttribute<SqlTableAttribute>(false);
            
            var attr = type.CustomAttributes.Where(data => data.AttributeType == typeof(SqlTableAttribute));
            if (attr != null && attr.Any())
            {

                var value = attr.First().NamedArguments.First(arg => arg.MemberName == "TableName").TypedValue.Value?.ToString();
                if (string.IsNullOrEmpty(value))
                    return GetFullName( includeDatabase, includeSchema, includeBrackets);
                return GetFullName(value, includeDatabase, includeSchema, includeBrackets);
            }
            else
            {
                //  return typeof(T).Name;
                return GetFullName(type.Name, includeDatabase, includeSchema, includeBrackets);
            }
        }

        public string GetFullName(bool includeDatabase = true, bool includeSchema = true, bool includeKeywordEscapeCharacters = true)
        {
            var fullTableName = "";
            if (!string.IsNullOrEmpty(DatabaseName))
            {
                fullTableName += $"{includeKeywordEscapeCharacters ? DatabaseName :}.";
            }
            if (!string.IsNullOrEmpty(SchemaName))
            {
                fullTableName += $"{SchemaName}.";
            }
            else
            {
                return TableName;
            }
            return $"{fullTableName}{TableName}";
        }


        
    }
}
