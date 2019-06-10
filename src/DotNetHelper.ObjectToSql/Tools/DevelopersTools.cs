using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DotNetHelper.ObjectToSql.Enum;
using Newtonsoft.Json;

namespace DotNetHelper.ObjectToSql.Tools
{
    public static class DevelopersTools
    {
        public static string ToCSharpClass(IDataReader reader, string className = null, bool nullifyProperties = false)
        {
            if (reader.IsClosed)
            {
                return null;
            }
            var sb = new StringBuilder();
            try
            {
                className = string.IsNullOrEmpty(className) ? "Dynamic Class" : className;
                sb.AppendLine($"public class {className} {{ ");
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    sb.AppendLine(nullifyProperties ? $"public {reader.GetFieldType(i).Name}? {reader.GetName(i)} {{ get; set; }} = null "
                        : $"public {reader.GetFieldType(i).Name} {reader.GetName(i)} {{ get; set; }} ");
                }
                sb.AppendLine($"}}");
                reader.Close();
                reader.Dispose();
            }
            catch (Exception e)
            {
                reader.Close();
                reader.Dispose();
                throw e;

            }
            return sb.ToString();
        }

        //  public static string ToCSharpClass(object obj, string className = null, bool nullifyProperties = false)
        //  {
        //      var list = new List<object>() { obj };
        //      var reader = ObjectReader.Create(list);
        //      return ToCSharpClass(reader, className, nullifyProperties);
        //  }

        public static string XmlTCSharpClass(string xml)
        {
            var doc = XDocument.Parse(xml); //or XDocument.Load(path)
            string jsonText = JsonConvert.SerializeXNode(doc);
            dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            var className = doc.Root.Name.LocalName;
            var props = (IDictionary<string, object>)dyn;

            var sb = new StringBuilder();
            try
            {
                className = string.IsNullOrEmpty(className) ? "Dynamic Class" : className;
                sb.AppendLine($"public class {className} {{ ");
                for (var i = 0; i < props.Count; i++)
                {
                    sb.AppendLine($"public {props.Values.ToList()[i].GetType().Name} {props.Keys.ToList()[i]} {{ get; set; }} ");
                }
                sb.AppendLine($"}}");
            }
            catch (Exception)
            {
              
            }
            return sb.ToString();
        }
  





        //public static string AdvanceTypeToSqlType(IMember advance, DataBaseType sqlType, bool hasMultipleKeys = false)
        //{
        //    var str = TypeToSqlType(advance.Type, sqlType);
        //    if (advance.Type == typeof(string) && advance.GetCustomAttribute<SqlColumnAttribute>()?.PrimaryKey == true)
        //    {
        //        str = str.Replace("(MAX)", "(900)"); /// SQL SERVER Doesn't allow varchar max to be primary key must be 900 bytes or less
        //    }


        //    var sqlColumnAttribute = advance.GetCustomAttribute<SqlColumnAttribute>();
        //    var allowIdentity = true;
        //    enumSql.ForEach(delegate (SqlColumnAttributeMembers members)
        //    {
        //        switch (members)
        //        {
        //            case SqlColumnAttributeMembers.SetMaxColumnSize:
        //                if (advance.GetCustomAttribute<SqlColumnAttribute>()?.MaxColumnSize != null)
        //                {
        //                    //  if (sqlType == DataBaseType.Sqlite)
        //                    str = str.Replace(" (MAX)", $"({advance.GetCustomAttribute<SqlColumnAttribute>()?.MaxColumnSize})");
        //                }
        //                break;
        //            case SqlColumnAttributeMembers.SetNullable:
        //                if (advance.GetCustomAttribute<SqlColumnAttribute>()?.Nullable == false)
        //                {
        //                    str += ($" NOT NULL ");
        //                }
        //                else if (advance.GetCustomAttribute<SqlColumnAttribute>()?.Nullable == true)
        //                {
        //                    str += ($" NULL ");
        //                }
        //                break;
        //            case SqlColumnAttributeMembers.SetAutoIncrementBy:
        //            case SqlColumnAttributeMembers.SetStartIncrementAt:
        //                if (sqlType == DataBaseType.Access95) break;
        //                if (sqlType == DataBaseType.Sqlite) break;
        //                if (allowIdentity)
        //                {
        //                    if (advance.GetCustomAttribute<SqlColumnAttribute>()?.AutoIncrementBy != null ||
        //                        advance.GetCustomAttribute<SqlColumnAttribute>()?.StartIncrementAt != null)
        //                        str +=  $" IDENTITY({advance.GetCustomAttribute<SqlColumnAttribute>()?.StartIncrementAt.GetValueOrDefault(1)},{advance.GetCustomAttribute<SqlColumnAttribute>()?.AutoIncrementBy.GetValueOrDefault(1)})";
        //                }

        //                allowIdentity = false;

        //                break;
        //            case SqlColumnAttributeMembers.SetUtcDateTime:

        //                break;
        //            case SqlColumnAttributeMembers.SetPrimaryKey:
        //                if (sqlType == DataBaseType.Access95) break;
        //                if (advance.GetCustomAttribute<SqlColumnAttribute>()?.PrimaryKey != null && advance.GetCustomAttribute<SqlColumnAttribute>()?.PrimaryKey == true && !hasMultipleKeys)
        //                    str += ($" PRIMARY KEY ");
        //                break;
        //            case SqlColumnAttributeMembers.SetApiId:

        //                break;
        //            case SqlColumnAttributeMembers.SetSyncTime:

        //                break;
        //            case SqlColumnAttributeMembers.SetIgnore:

        //                break;
        //            case SqlColumnAttributeMembers.MapTo:
        //                break;
        //            case SqlColumnAttributeMembers.DefaultValue:
        //                break;
        //            case SqlColumnAttributeMembers.TSQLDefaultValue:

        //                if(!string.IsNullOrEmpty(advance.GetCustomAttribute<SqlColumnAttribute>()?.TSQLDefaultValue))
        //                str += ($" DEFAULT {advance.GetCustomAttribute<SqlColumnAttribute>()?.TSQLDefaultValue} ");
        //                break;
        //            case SqlColumnAttributeMembers.SetxRefTableType:
        //                break;
        //            case SqlColumnAttributeMembers.xRefTableSchema:
        //                break;
        //            case SqlColumnAttributeMembers.xRefTableName:
        //                if (!string.IsNullOrEmpty(advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefTableName) && !string.IsNullOrEmpty(advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefJoinOnColumn))
        //                {
        //                    str += ($" FOREIGN KEY REFERENCES {advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefTableName}({advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefJoinOnColumn}) ");
        //                    if (advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefOnDeleteCascade.GetValueOrDefault(false) == true)
        //                        str += ($" ON DELETE CASCADE ");
        //                    if (advance.GetCustomAttribute<SqlColumnAttribute>()?.xRefOnUpdateCascade.GetValueOrDefault(false) == true)
        //                        str += ($" ON UPDATE CASCADE ");
        //                }
        //                break;
        //            case SqlColumnAttributeMembers.xRefJoinOnColumn:
        //                break;
        //            case SqlColumnAttributeMembers.SetxRefOnUpdateCascade:
        //                break;
        //            case SqlColumnAttributeMembers.SetxRefOnDeleteCascade:
        //                break;
        //            case SqlColumnAttributeMembers.MappingIds:
        //                // WILL NEVER DO ANYTHING THIS IS FOR JOIN PURPOSE ONLY
        //                break;
        //            case SqlColumnAttributeMembers.SerializableType:

        //                break;
        //        }
        //    });


        //    return str;
        //}


        // https://www.tutorialspoint.com/sqlite/sqlite_data_types.htm
        public static string TypeToSqlType(Type type, DataBaseType sqlType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);

            }

            if (type == typeof(string) || type == typeof(object))
            {
                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"VARCHAR (MAX)");
                    case DataBaseType.MySql:
                        return ($"VARCHAR (MAX)");
                    case DataBaseType.Sqlite:
                        return ($"TEXT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"TEXT"); // VARCHARS WORKS BUT WE WON'T DEFAULT TO IT BECAUSE IT ONLY SUPPORTS UP TO 255 CHARACTERS
                    case DataBaseType.Odbc:
                        return ($"VARCHAR (MAX)");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }

            }
            else if (type == typeof(int) || type == typeof(System.Enum) || type == typeof(short))
            {
                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"INT");
                    case DataBaseType.MySql:
                        return ($"INT");
                    case DataBaseType.Sqlite:
                        return ($"INTEGER");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"INTEGER");
                    case DataBaseType.Odbc:
                        return ($"INT");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }

            }
            else if (type == typeof(long))
            {
                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"BIGINT");
                    case DataBaseType.MySql:
                        return ($"BIGINT");
                    case DataBaseType.Sqlite:
                        return ($"BIGINT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"INTEGER");
                    case DataBaseType.Odbc:
                        return ($"BIGINT");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }

            }


            else if (type == typeof(DateTime))
            {

                switch (sqlType)
                {

                    case DataBaseType.SqlServer:
                        return ($"DATETIME");
                    case DataBaseType.MySql:
                        return ($"DATETIME");
                    case DataBaseType.Sqlite:
                        return ($"TEXT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"DATETIME");
                    case DataBaseType.Odbc:
                        return ($"DATETIME");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }

            else if (type == typeof(DateTimeOffset)) // TODO :: NEED TO VALIDATE OTHER DATEBASE TYPE OTHER THAN SQLSERVER 
            {

                switch (sqlType)
                {

                    case DataBaseType.SqlServer:
                        return ($"DATETIMEOFFSET");
                    case DataBaseType.MySql:
                        return ($"DATETIMEOFFSET");
                    case DataBaseType.Sqlite:
                        return ($"TEXT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"DATETIMEOFFSET");
                    case DataBaseType.Odbc:
                        return ($"DATETIMEOFFSET");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(bool))
            {
                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"BIT");
                    case DataBaseType.MySql: // SUPPORTS MySQL 5.0.3 & HIGHER ONLY 
                        return ($"BIT");
                    case DataBaseType.Sqlite:
                        return ($"BIT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"BIT");
                    case DataBaseType.Odbc:
                        return ($"BIT");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(double))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"FLOAT");
                    case DataBaseType.MySql:
                        return ($"DOUBLE");
                    case DataBaseType.Sqlite:
                        return ($"REAL");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"DOUBLE");
                    case DataBaseType.Odbc:
                        return ($"FLOAT");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(decimal))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"DECIMAL (18,2)");
                    case DataBaseType.MySql:
                        return ($"DECIMAL (18,2)");
                    case DataBaseType.Sqlite:
                        return ($"NUMERIC");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"CURRENCY");
                    case DataBaseType.Odbc:
                        return ($"DECIMAL (18,2)");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(byte))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"TINYINT");
                    case DataBaseType.MySql:
                        return ($"TINYINT");
                    case DataBaseType.Sqlite:
                        return ($"TINYINT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"SMALLINT");
                    case DataBaseType.Odbc:
                        return ($"TINYINT");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(byte[]))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"VARBINARY (MAX)");
                    case DataBaseType.MySql:
                        return ($"BLOB");
                    case DataBaseType.Sqlite:
                        return ($"BLOB");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"VARBINARY");
                    case DataBaseType.Odbc:
                        return ($"VARBINARY (MAX)");

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(Guid))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"uniqueidentifier");
                    case DataBaseType.MySql:
                        return ($"CHAR(16)");
                    case DataBaseType.Sqlite:
                        break;
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"VARCHAR(32)");
                    case DataBaseType.Odbc:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }
            else if (type == typeof(char))
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"CHARACTER");
                    case DataBaseType.MySql:
                        return ($"CHAR");
                    case DataBaseType.Sqlite:
                        return ($"TEXT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"CHAR");
                    case DataBaseType.Odbc:
                        return ($"CHAR");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }

            else
            {

                switch (sqlType)
                {
                    case DataBaseType.SqlServer:
                        return ($"VARCHAR (MAX)");
                    case DataBaseType.MySql:
                        return ($"VARCHAR (MAX)");
                    case DataBaseType.Sqlite:
                        return ($"TEXT");
                    case DataBaseType.Oracle:
                        break;
                    case DataBaseType.Oledb:
                        break;
                    case DataBaseType.Access95:
                        return ($"TEXT");
                    case DataBaseType.Odbc:
                        return ($"VARCHAR(50000)");
                    default:
                        throw new ArgumentOutOfRangeException(nameof(sqlType), sqlType, null);
                }
            }


            return null;
        }





       




    }
}