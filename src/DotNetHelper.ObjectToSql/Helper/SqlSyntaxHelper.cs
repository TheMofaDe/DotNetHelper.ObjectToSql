using System;
using System.Collections.Generic;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;

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


        public string GetTableOpenChar()
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
        public string GetTableClosedChar()
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
        public string RemoveBracketsChar(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace(GetTableOpenChar(), string.Empty).Replace(GetTableClosedChar(), string.Empty);

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




   

    }




}
