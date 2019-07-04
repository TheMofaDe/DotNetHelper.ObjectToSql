using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
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
                    return "`";
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
                    return "`";
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
                    return $@"IF ( {selectStatement} ) THEN BEGIN {onTrueSql} END; ELSE BEGIN {onFalseSql} END; END IF;";
                case DataBaseType.Sqlite:
                    // https://stackoverflow.com/questions/418898/sqlite-upsert-not-insert-or-replace/4330694
                    //  throw new NotSupportedException("SQLITE d");
                    //  return $@"CASE WHEN ( {selectStatement} ) THEN {onTrueSql} ELSE  {onFalseSql} ;";
                    return null;
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

        public string BuildTableExistStatement(SQLTable sqlTable, string onTrueSql, string onFalseSql)
        {

            var query = $"";
            switch (DataBaseType)
            {
                case DataBaseType.SqlServer:
                    query = $"IF OBJECT_ID(N'{sqlTable.FullNameWithBrackets}', N'U') IS NOT NULL BEGIN {onTrueSql} END ELSE BEGIN {onFalseSql} END"; 
                    break;
                case DataBaseType.MySql:
                    query = $"SELECT CASE WHEN COUNT(*) > 0 THEN 'TRUE' ELSE 'FALSE' END FROM information_schema.tables " +
                            $" WHERE table_schema = '{sqlTable.SchemaName}'" +
                            $" AND table_name = '{sqlTable.TableName}' " +
                            $" LIMIT 1;";
                    break;
                case DataBaseType.Sqlite:
                    query = $"SELECT CASE WHEN COUNT(( SELECT name FROM sqlite_master WHERE type='table' AND name='{sqlTable.TableName}')) > 0 THEN 'TRUE' ELSE 'FALSE' END AS WHOCARES";
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

            return query;
        }




        public string ConvertParameterSqlToReadable(List<DbParameter> parameters, string query,Encoding encoding)
        {
            var sql = query.Clone().ToString();
            // Convert Query To Human Readable  

            var orderedParameters = parameters.OrderByDescending(x => x.ParameterName).ToList();
            orderedParameters.ForEach(delegate (DbParameter parameter)
            {
                var name = parameter.ParameterName;
                sql = sql.Replace(name.StartsWith("@") ? $"{name}" : $"@{name}", CommandToSQl(parameter.Value, encoding ?? Encoding.UTF8));
            });

            return sql;

        }


        private  string CommandToSQl(object obj, Encoding encoding)
        {

            if (obj == null || obj == DBNull.Value)
            {
                return "NULL";
            }
            else if (obj is byte[] byteArray)
            {
                var quoteField = EnclosedValueLookup[byteArray.GetType()];
                return byteArray.Length <= 0 ? $@"NULL" : $@"{quoteField}{encoding.GetString(byteArray)}{quoteField}";
            }
            else if (obj is string stringValue)
            {
                var quoteField = EnclosedValueLookup[stringValue.GetType()];
                return $@"{quoteField}{stringValue.Replace("'", "''")}{quoteField}"; 
            }
            else if (obj is char charValue)
            {
                var quoteField = EnclosedValueLookup[charValue.GetType()];
                return $@"{quoteField}{charValue.ToString().Replace("'", "''")}{quoteField}";
            }
            else if (obj is int intValue)
            {
                return $@"{intValue}";
            }
            else if (obj is byte byteValue)
            {
                return $@"{byteValue}";
            }
            else if (obj is float floatValue)
            {
                return $@"{floatValue}";
            }
            else if (obj is long longValue)
            {
                return $@"{longValue}";
            }
            else if (obj is double doubleValue)
            {
                return $@"{doubleValue}";
            }
            else if (obj is short shortValue)
            {
                return $@"{shortValue}";
            }
            else if (obj is decimal decimalValue)
            {
                return $@"{decimalValue}";
            }
            else if (obj is DateTime dateTimeValue)
            {
                var quoteField = EnclosedValueLookup[dateTimeValue.GetType()];
                return $@"{quoteField}{dateTimeValue}{quoteField}";
            }
            else if (obj is DateTimeOffset dateTimeOffsetValue)
            {
                var quoteField = EnclosedValueLookup[dateTimeOffsetValue.GetType()];
                return $@"{quoteField}{dateTimeOffsetValue}{quoteField}";
            }
            else if (obj is TimeSpan timespanValue)
            {
                var quoteField = EnclosedValueLookup[timespanValue.GetType()];
                return $@"{quoteField}{timespanValue}{quoteField}";
            }
            else if (obj is bool booleanValue)
            {
                return booleanValue ? $"1" : $"0"; 
            }
            else if (obj is Guid guidValue)
            {
                var quoteField = EnclosedValueLookup[guidValue.GetType()];
                return $@"CAST({quoteField}{guidValue}{quoteField} AS UNIQUEIDENTIFIER)";
            }
            else
            {
                // We Convert Non System Types To Json
                return $"NULL";

            }
        }






    }




}
