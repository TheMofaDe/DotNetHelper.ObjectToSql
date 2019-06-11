using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;

namespace DotNetHelper.ObjectToSql.Helper
{
    public class ObjectToSqlHelper
    {

        public ObjectToSqlHelper()
        {
           
        }


        /// <summary>
        /// Builds the where clause.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="keyFields">The key fields.</param>
        
        public static void BuildWhereClause(StringBuilder sqlBuilder, List<MemberWrapper> keyFields)
        {

    
            if (keyFields.IsNullOrEmpty())
            {
               // throw new InvalidOperationException("Can't Build Where Clause Or Perform Upsert And Update Statements Without Having At Least One Property That Inherits The SqlColumnAttribute & Have The PRIMARY KEY SET TO TRUE OR The Foreign Key Set Up");
               return;
            }
            else
            {
                sqlBuilder.Append("WHERE");
                keyFields.ForEach(p => sqlBuilder.Append($" [{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name} AND"));
                if (sqlBuilder.ToString().EndsWith(" AND"))
                    sqlBuilder.Remove(sqlBuilder.Length - 4, 4); // Remove the last AND       
            }

        }

//        Func<string, object, DbParameter> GetNewParameter
        /// <summary>
        /// Builds the where clause.Dyn
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="keyFields">The key fields.</param>
        
        public List<DbParameter> BuildWhereClauseAndGetDbParameters<T>(T obj, Func<string,object, DbParameter>  GetNewParameter, StringBuilder sqlBuilder, List<MemberWrapper> keyFields, bool throwOnNoAttributes) where T: class
        {
            var list = new List<DbParameter>() { };
            if (throwOnNoAttributes && !keyFields.Where(m => m.GetCustomAttribute<SqlColumnAttribute>()?.PrimaryKey == true || !string.IsNullOrEmpty(m.GetCustomAttribute<SqlColumnAttribute>()?.xRefTableName)).ToList().Any())
            {
                throw new InvalidOperationException("Can't Build Where Clause Or Perform Upsert And Update Statements Without Having At Least One Property That Inherits The SqlColumnAttribute & Have The PRIMARY KEY SET TO TRUE OR The Foreign Key Set Up");
            }
            else
            {
                sqlBuilder.Append("WHERE");
                keyFields.ForEach(delegate (MemberWrapper p)
                {
                    sqlBuilder.Append($" [{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name} AND");
                    list.Add(GetNewParameter(p.Name, p.GetValue(obj)));
                });
                if (sqlBuilder.ToString().EndsWith(" AND"))
                    sqlBuilder.Remove(sqlBuilder.Length - 4, 4); // Remove the last AND       

            }
            return list;
        }



        /// <summary>
        /// Gets the non key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields<T>(bool includeNonPublicAccessor) where T : class
        {
            // Get non primary key fields - the ones we want to update.
                return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.IsMemberAPrimaryKeyColumn()).ToList();
        }
        /// <summary>
        /// Gets the non key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields<T>(T dynamicObject) where T : IDynamicMetaObjectProvider
        {
            // Get non primary key fields - the ones we want to update.
            return ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => !m.IsMemberAPrimaryKeyColumn()).ToList();
        }


        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetKeyFields<T>(bool includeNonPublicAccessor) where T : class
        {
                // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
                return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).ToList();
        }

        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetKeyFields<T>(T dynamicObject) where T : IDynamicMetaObjectProvider
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            return ExtFastMember.GetMemberWrappers(dynamicObject).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).ToList();
        }


        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetKeyFields( bool includeNonPublicAccessor, Type type)
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            return ExtFastMember.GetMemberWrappers(type,includeNonPublicAccessor).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }



        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetNonIdentityFields<T>(bool includeNonPublicAccessor) where T : class
        {
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }

        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetNonIdentityFields<T>(T dynamicObject) where T : IDynamicMetaObjectProvider
        {
            return ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }

        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetNonIdentityFields( bool includeNonPublicAccessor, Type type) 
        {
            return ExtFastMember.GetMemberWrappers(type,includeNonPublicAccessor).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }



        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public  List<MemberWrapper> GetAllNonIgnoreFields<T>(bool includeNonPublicAccessor) where T : class
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            var temp = ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.ShouldMemberBeIgnored()).AsList();
            return temp;
        }
        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetAllNonIgnoreFields<T>(T dynamicObject) where T : IDynamicMetaObjectProvider
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            var temp = ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => !m.ShouldMemberBeIgnored()).AsList();
            return temp;
        }


        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public static List<MemberWrapper> GetAllNonIgnoreFields(Type type, bool includeNonPublicAccessor)
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            var temp = ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => !m.ShouldMemberBeIgnored()).AsList();
            return temp;
        }



   

        /// <summary>
        /// Converts to database value.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object ConvertToDatabaseValue(MemberWrapper member, object value, Func<object,string> XmlSerializer, Func<object, string> JsonSerializer, Func<object, string> CsvSerializer)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (member.Type == typeof(DateTime) && (DateTime)value == DateTime.MinValue || member.Type == typeof(DateTime?) && (DateTime)value == DateTime.MinValue)
            {
                return new DateTime(1753, 01, 01);
            }
            if (member.GetCustomAttribute<SqlColumnAttribute>()?.SerializableType != SerializableType.NONE)
            {
                    switch (member.GetCustomAttribute<SqlColumnAttribute>()?.SerializableType)
                    {
                        case SerializableType.XML:
                            XmlSerializer.IsNullThrow(nameof(XmlSerializer), new ArgumentNullException(nameof(XmlSerializer),$"{ExceptionHelper.NullSerializer(member,SerializableType.XML)}"));
                            return XmlSerializer.Invoke(value);
                        case SerializableType.JSON:
                            JsonSerializer.IsNullThrow(nameof(JsonSerializer), new ArgumentNullException(nameof(JsonSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.JSON)}"));
                            return JsonSerializer.Invoke(value);
                        case SerializableType.CSV:
                            CsvSerializer.IsNullThrow(nameof(CsvSerializer), new ArgumentNullException(nameof(CsvSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.CSV)}"));
                            return CsvSerializer.Invoke(value);
                        case SerializableType.NONE:
                        case null:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            return value;
        }



        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco">The poco.</param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<DbParameter> BuildDbParameterList<T>(T poco, Func<string, object, DbParameter> GetNewParameter, Func<object, string> XmlSerializer, Func<object, string> JsonSerializer, Func<object, string> CsvSerializer, bool includeNonPublicAccessor) where T : class
        {
            var list = new List<DbParameter>() { };
            ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).ForEach(delegate (MemberWrapper p)
            {
                var  parameterValue = ConvertToDatabaseValue(p, p.GetValue(poco),XmlSerializer,JsonSerializer,CsvSerializer);
                list.Add(GetNewParameter($"@{p.Name}", parameterValue));
            });
            return list;
        }


       



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableAlias"></param>
        /// <param name="MemberWrappers"></param>
        /// <param name="sqlSyntax"></param>
        /// <returns>item 1 is the actual columns being selected
        ///          item 2 is the split on column</returns>
        internal Tuple<string,string> BuildSelectColumnStatement(char tableAlias,List<MemberWrapper> MemberWrappers, SqlSyntaxHelper sqlSyntax)
        {
            var sb = new StringBuilder();
            var isFirstTime = true;
            var splitOn = "";
            MemberWrappers.Where(a1 => a1.GetCustomAttribute<SqlTableAttribute>() == null && a1.GetCustomAttribute<SqlColumnAttribute>()?.Ignore != true).ToList().ForEach(delegate (MemberWrapper member) // BUILD SQL COLUMNS
            {
                var columnName = $"{tableAlias}.{sqlSyntax.GetTableOpenChar()}{member.Name}{sqlSyntax.GetTableClosedChar()}";
                sb.AppendLine($"{columnName} , ");
                if (isFirstTime)
                {
                    splitOn = member.Name;
                    isFirstTime = false;
                }
            });
            return new Tuple<string, string>(sb.ToString(),splitOn); 
        }

        internal void BuildJoinOnStatement(List<MemberWrapper> members, char mainTableAlias, List<MemberWrapper> members1, char secondTableAlias, StringBuilder sqlFromBuilder)
        {
            var safeKeyword = " AND ";
            members.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true && a.GetCustomAttribute<SqlTableAttribute>() == null).ToList().ForEach(delegate (MemberWrapper mainTableColumn) // LOOP THRU MAIN TABLE PROPERTIES 
            {

                members1.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true).ToList().ForEach(delegate(MemberWrapper secondTableColumn)
                {

                    var attr = mainTableColumn.GetCustomAttribute<SqlColumnAttribute>();
                    var attr2 = mainTableColumn.GetCustomAttribute<SqlColumnAttribute>();
                    if (attr?.MappingIds.ContainAnySameItem(attr2?.MappingIds) == true)
                    {
                      
                      
                            sqlFromBuilder.Append($" {mainTableAlias}.{mainTableColumn.Name} " +
                                                  $"= {secondTableAlias}.{secondTableColumn.Name} ");
                            sqlFromBuilder.Append(safeKeyword);
                        
                       // var iHateThis = sqlFromBuilder.ToString().ReplaceLastOccurrance(safeKeyword, string.Empty, StringComparison.Ordinal);
                       // sqlFromBuilder.Clear();
                       //  sqlFromBuilder.Append(sqlFromBuilder.ToString().ReplaceLastOccurrance(safeKeyword, string.Empty, StringComparison.Ordinal));
                       // sqlFromBuilder.Clear();
                    }
                });
 
            });

                sqlFromBuilder = sqlFromBuilder.ReplaceLastOccurrence(safeKeyword, string.Empty, StringComparison.Ordinal);

        }


        //internal Dictionary<Type, char> GetTableAliasRecursive<T>() where T : class
        //{

        //    var currentAlias = 'A';
        //    var lookup = new Dictionary<Type, char>()
        //    {
        //         {   typeof(T),currentAlias  }
        //    };


        //    void addAlias(MemberWrapper m)
        //    {
        //        if (lookup.ContainsKey(m.Type))
        //        {

        //        }
        //        else
        //        {
        //            currentAlias = AlphabetHelper.GetNextLetter(currentAlias);
        //            lookup.Add(m.Type, currentAlias);
        //        }
               
        //    }

            

        //    void getTableMembers(Type type)
        //    {
        //        ExtFastMember.GetMemberWrappers(type).Where(m => m.GetCustomAttribute<SqlTableAttribute>() != null).ToList().ForEach(delegate (MemberWrapper m)
        //        {
        //            addAlias(m);
        //            getTableMembers(m.Type);
        //        });
        //    }

            
      
        //    getTableMembers(typeof(T));


        //    return lookup;
        //}

    }
}
