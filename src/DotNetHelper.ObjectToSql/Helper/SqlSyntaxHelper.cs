using System;
using System.Collections.Generic;
using System.Linq;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Model;

namespace DotNetHelper.ObjectToSql.Helper
{
    public class SqlSyntaxHelper

    {
        public Dictionary<Type,string> EnclosedValueLookup { get; }
        public DataBaseType DataBaseType { get; }
        public SqlSyntaxHelper(DataBaseType type)
        {
            DataBaseType = type;
            switch (type)
            {
                case DataBaseType.SqlServer:
                    EnclosedValueLookup = new Dictionary<Type, string>()
                    {
                        {typeof(int), string.Empty},
                        {typeof(Guid), "'"},
                        {typeof(DateTime), "'"},
                        {typeof(DateTimeOffset), "'"},
                        {typeof(TimeSpan), "'"},
                        {typeof(long), string.Empty},
                        {typeof(bool), string.Empty},
                        {typeof(double), string.Empty},
                        {typeof(short), string.Empty},
                        {typeof(decimal), string.Empty},
                        {typeof(float), string.Empty},
                        {typeof(byte), "'"},
                        {typeof(char), "'"},
                        {typeof(uint), string.Empty},
                        {typeof(ushort), string.Empty},
                        {typeof(ulong), string.Empty},
                        {typeof(sbyte), "'"},
                        {typeof(int?), string.Empty},
                        {typeof(Guid?), "'"},
                        {typeof(DateTime?), "'"},
                        {typeof(DateTimeOffset?), "'"},
                        {typeof(TimeSpan?), "'"},
                        {typeof(long?), string.Empty},
                        {typeof(bool?), string.Empty},
                        {typeof(double?), string.Empty},
                        {typeof(decimal?), string.Empty},
                        {typeof(short?), string.Empty},
                        {typeof(float?), string.Empty},
                        {typeof(byte?), string.Empty},
                        {typeof(char?), "'"},
                        {typeof(uint?), string.Empty},
                        {typeof(ushort?), string.Empty},
                        {typeof(ulong?), string.Empty},
                        {typeof(sbyte?), string.Empty}

                    };
                    break;
                case DataBaseType.MySql:
                    break;
                case DataBaseType.Sqlite:
                    break;
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    EnclosedValueLookup = new Dictionary<Type, string>()
                    {
                        {typeof(int), string.Empty},
                        {typeof(Guid), "'"},
                        {typeof(DateTime), "#"},
                        {typeof(DateTimeOffset), "#"},
                        {typeof(TimeSpan), "'"},
                        {typeof(long), string.Empty},
                        {typeof(bool), string.Empty},
                        {typeof(double), string.Empty},
                        {typeof(short), string.Empty},
                        {typeof(decimal), string.Empty},
                        {typeof(float), string.Empty},
                        {typeof(byte), "'"},
                        {typeof(char), "'"},
                        {typeof(uint), string.Empty},
                        {typeof(ushort), string.Empty},
                        {typeof(ulong), string.Empty},
                        {typeof(sbyte), "'"},
                        {typeof(int?), string.Empty},
                        {typeof(Guid?), "'"},
                        {typeof(DateTime?), "#"},
                        {typeof(DateTimeOffset?), "#"},
                        {typeof(TimeSpan?), "#"},
                        {typeof(long?), string.Empty},
                        {typeof(bool?), string.Empty},
                        {typeof(double?), string.Empty},
                        {typeof(decimal?), string.Empty},
                        {typeof(short?), string.Empty},
                        {typeof(float?), string.Empty},
                        {typeof(byte?), string.Empty},
                        {typeof(char?), "'"},
                        {typeof(uint?), string.Empty},
                        {typeof(ushort?), string.Empty},
                        {typeof(ulong?), string.Empty},
                        {typeof(sbyte?), string.Empty}

                    };
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public string GetKeywordEscapeOpenChar()
        {
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return "[";
                case DataBaseType.MySql:
                    return "[";
                case DataBaseType.Sqlite:
                    return "[";
                case DataBaseType.Oracle:
                    return "[";
                case DataBaseType.Oledb:
                    return "[";
                case DataBaseType.Access95:
                    return "[";
                case DataBaseType.Odbc:
                    return "[";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public string GetKeywordEscapeClosedChar()
        {
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return "]";
                case DataBaseType.MySql:
                    return "]";
                case DataBaseType.Sqlite:
                    return "]";
                case DataBaseType.Oracle:
                    return "]";
                case DataBaseType.Oledb:
                    return "]";
                case DataBaseType.Access95:
                    return "]";
                case DataBaseType.Odbc:
                    return "]";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public string RemoveKeywordEscapeChars(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace(GetKeywordEscapeOpenChar(), string.Empty).Replace(GetKeywordEscapeClosedChar(), string.Empty);

        }


        public string GetEnclosedValueChar(Type type)
        {





#if NETFRAMEWORK
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.MySql:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.Sqlite:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.Oracle:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.Oledb:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.Access95:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                case DataBaseType.Odbc:
                    return EnclosedValueLookup.GetValueOrDefault(type, string.Empty);
                default:
                    throw new ArgumentOutOfRangeException();
            }
#else
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.MySql:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.Sqlite:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.Oracle:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.Oledb:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.Access95:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                case DataBaseType.Odbc:
                    return EnclosedValueLookup.GetValueOrDefaultValue(type, string.Empty);
                default:
                    throw new ArgumentOutOfRangeException();
            }
#endif
        }



        public string BuildIfExistStatement(string selectStatement,string onTrueSql,string onFalseSql)
        {
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return $"IF EXISTS ( {selectStatement} ) BEGIN {onTrueSql} END ELSE BEGIN {onFalseSql} END";
                case DataBaseType.MySql:
                    break;
                case DataBaseType.Sqlite:
                    break;
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return null;
        }







        internal SQLTable GetSqlTableFromString(string tableName, bool includeBrackets)
        {
            var tableNameParseObject = new SQLTable();
            if (tableName.Contains("."))
            {
                var splits = tableName.Split('.');
                if (splits.Length == 3) // database.schema.table
                {
                    tableNameParseObject.DatabaseName = includeBrackets ? AddBrackets(splits[0]) : RemoveBrackets(splits[0]);
                    tableNameParseObject.SchemaName = includeBrackets ? AddBrackets(splits[1]) : RemoveBrackets(splits[1]);
                    tableNameParseObject.TableName = includeBrackets ? AddBrackets(splits[2]) : RemoveBrackets(splits[2]);
                }
                else if (splits.Length == 2) // schema.table
                {
                    tableNameParseObject.SchemaName = includeBrackets ? AddBrackets(splits[0]) : RemoveBrackets(splits[0]);
                    tableNameParseObject.TableName = includeBrackets ? AddBrackets(splits[1]) : RemoveBrackets(splits[1]);

                }
                else if (splits.Length == 1) // .table
                {
                    tableNameParseObject.TableName = includeBrackets ? AddBrackets(splits[0]) : RemoveBrackets(splits[0]);

                }
            }
            else
            {
                tableNameParseObject.TableName = tableName;
            }
            return tableNameParseObject;

        }


        ///// <summary>
        ///// Gets the full name of the table.
        ///// </summary>
        ///// <param name="tableName">Name of the table.</param>
        ///// <param name="includeSchema"></param>
        ///// <param name="includeBrackets"></param>
        ///// <param name="includeDatabase"></param>
        ///// <returns>System.String.</returns>
        private string FormatTableNameString(string tableName, bool includeDatabase, bool includeSchema, bool includeBrackets)
        {
            if (string.IsNullOrEmpty(tableName)) return "";

            var obj = GetParseObject(tableName, includeBrackets);
            var db = string.IsNullOrEmpty(obj.DatabaseName) ? "" : $"{obj.DatabaseName}.";
            var schema = string.IsNullOrEmpty(obj.SchemaName) ? "" : $"{obj.SchemaName}.";
            var table = string.IsNullOrEmpty(obj.TableName) ? "" : $"{obj.TableName}";
            if (includeDatabase && includeSchema)
            {
                return $"{db}{schema}{table}";
            }
            else if (!includeDatabase && includeSchema)
            {
                return $"{schema}{table}";
            }
            else if (includeDatabase)
            {
                return $"{db}{table}";
            }
            else
            {
                return $"{obj.TableName}";
            }


        }


        public virtual string GetDefaultTableName<T>(string tableName, bool includeDatabase = true, bool includeSchema = true, bool includeBrackets = true)
        {

            if (!string.IsNullOrEmpty(tableName)) // developer passed in table name
            {
                return FormatTableNameString(tableName, includeDatabase, includeSchema, includeBrackets);
            }
            var type = typeof(T);
            while (type.IsTypeAnIEnumerable())
            {
                type = type.GetEnumerableItemType();
            }


            var attr = type.CustomAttributes.Where(data => data.AttributeType == typeof(SqlTableAttribute));
            if (!attr.IsNullOrEmpty())
            {

                var value = attr.First().NamedArguments.First(arg => arg.MemberName == "TableName").TypedValue.Value.ToString();
                return FormatTableNameString(value, includeDatabase, includeSchema, includeBrackets);
            }
            else
            {
                //  return typeof(T).Name;

                return FormatTableNameString(typeof(T).Name, includeDatabase, includeSchema, includeBrackets);
            }
        }

        public virtual string GetDefaultTableName(Type type, string tableName, bool includeDatabase = true, bool includeSchema = true, bool includeBrackets = true)
        {

            if (!string.IsNullOrEmpty(tableName)) // developer passed in table name
            {
                return FormatTableNameString(tableName, includeDatabase, includeSchema, includeBrackets);
            }


            while (type.IsTypeAnIEnumerable())
            {
                type = type.GetEnumerableItemType();
            }

            var attr = type.CustomAttributes.Where(data => data.AttributeType == typeof(SqlTableAttribute));
            if (!attr.IsNullOrEmpty())
            {

                var value = attr.First().NamedArguments.First(arg => arg.MemberName == "TableName").TypedValue.Value?.ToString();
                if (string.IsNullOrEmpty(value))
                    return FormatTableNameString(type.Name, includeDatabase, includeSchema, includeBrackets);
                return FormatTableNameString(value, includeDatabase, includeSchema, includeBrackets);
            }
            else
            {
                //  return typeof(T).Name;
                return FormatTableNameString(type.Name, includeDatabase, includeSchema, includeBrackets);
            }
        }

        public virtual string GetDefaultTableName(string tableName, bool includeDatabase = true, bool includeSchema = true, bool includeBrackets = true)
        {

            if (!string.IsNullOrEmpty(tableName)) // developer passed in table name
            {
                return FormatTableNameString(tableName, includeDatabase, includeSchema, includeBrackets);
            }
            throw new InvalidOperationException("Table Name is missing");
        }

    }




}
