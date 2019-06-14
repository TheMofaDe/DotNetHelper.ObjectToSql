using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql.Services
{
    public class ObjectToSql
    {
        
        public bool IncludeNonPublicAccessor { get; set; } = true;
        public DataBaseType DatabaseType { get; }

        public ObjectToSql(DataBaseType type,bool includeNonPublicAccessor)
        {
            DatabaseType = type;
            IncludeNonPublicAccessor = includeNonPublicAccessor;
        }
        public ObjectToSql(DataBaseType type)
        {
            DatabaseType = type;
        }


        public string BuildQuery<T>(string tableName, ActionType actionType, T instance) where T : class
        {

            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    if (instance is IDynamicMetaObjectProvider dynamicInsert)
                    {
                        BuildInsertQuery(dynamicInsert, sqlBuilder,tableName);
                    }
                    else
                    {
                        BuildInsertQuery<T>(sqlBuilder, tableName);
                    }
                    break;
                case ActionType.Update:
                    if (instance is IDynamicMetaObjectProvider dynamicUpdate)
                    {
                        BuildUpdateQuery(dynamicUpdate, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildUpdateQuery<T>(sqlBuilder, tableName);
                    }
                    break;
                case ActionType.Upsert:
                    if (instance is IDynamicMetaObjectProvider dynamicUpsert)
                    {
                        BuildUpsertQuery(dynamicUpsert, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildUpsertQuery<T>(sqlBuilder, tableName);
                    }
                    break;
                case ActionType.Delete:
                    if (instance is IDynamicMetaObjectProvider dynamicDelete)
                    {
                        BuildDeleteQuery(dynamicDelete, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildDeleteQuery<T>(sqlBuilder, tableName);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }

        public string BuildQuery(string tableName, ActionType actionType, object instance) 
        {
            instance.IsNullThrow(nameof(instance));
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    if (instance is IDynamicMetaObjectProvider dynamicInsert)
                    {
                        BuildInsertQuery(dynamicInsert, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildInsertQuery(sqlBuilder, tableName,instance.GetType());
                    }
                    break;
                case ActionType.Update:
                    if (instance is IDynamicMetaObjectProvider dynamicUpdate)
                    {
                        BuildUpdateQuery(dynamicUpdate, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildUpdateQuery(sqlBuilder, tableName, instance.GetType());
                    }
                    break;
                case ActionType.Upsert:
                    if (instance is IDynamicMetaObjectProvider dynamicUpsert)
                    {
                        BuildUpsertQuery(dynamicUpsert, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildUpsertQuery(sqlBuilder, tableName, instance.GetType());
                    }
                    break;
                case ActionType.Delete:
                    if (instance is IDynamicMetaObjectProvider dynamicDelete)
                    {
                        BuildDeleteQuery(dynamicDelete, sqlBuilder, tableName);
                    }
                    else
                    {
                        BuildDeleteQuery(sqlBuilder, tableName, instance.GetType());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }


        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildInsertQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {
            var allFields = ObjectToSqlHelper.GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? typeof(T).Name} (");

            // Add field names
            allFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault ()}],"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append(") VALUES (");
            allFields.ForEach(p => sqlBuilder.Append($"@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(")");
        }

        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildInsertQuery<T>(T poco, StringBuilder sqlBuilder, string tableName) where T : IDynamicMetaObjectProvider
        {
            var allFields = ObjectToSqlHelper.GetNonIdentityFields<T>(poco);
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? typeof(T).Name} (");

            // Add field names
            allFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}],"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append(") VALUES (");
            allFields.ForEach(p => sqlBuilder.Append($"@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(")");
        }

        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type"></param>
        public void BuildInsertQuery(StringBuilder sqlBuilder, string tableName,Type type) 
        {
            var allFields = ObjectToSqlHelper.GetNonIdentityFields(IncludeNonPublicAccessor, type);
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? type.Name} (");

            // Add field names
            allFields.ForEach(delegate(MemberWrapper member)
            {
                sqlBuilder.Append($"[{member.GetNameFromCustomAttributeOrDefault()}],"); 

            });
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append(") VALUES (");
            allFields.ForEach(p => sqlBuilder.Append($"@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(")");
        }


        /// <summary>
        /// Builds the insert query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="outFields"></param>
        public void BuildInsertQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {

            var outputFields = outFields.GetPropertyNamesFromExpressions();

            outputFields.IsEmptyThrow(nameof(outputFields));
            var allFields = ObjectToSqlHelper.GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            // Insert sql statement prefix 

            sqlBuilder.Append($"INSERT INTO {tableName} (");

            // Add field names
            allFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}],"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append($") {Environment.NewLine}");
            sqlBuilder.Append($" OUTPUT");

            var members = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor);
            outputFields.ForEach(delegate (string s) {
                sqlBuilder.Append($" INSERTED.[{members.FirstOrDefault(av => av.Name == s)?.GetNameFromCustomAttributeOrDefault() ?? s}] ,");
            });
            if (!outputFields.IsNullOrEmpty())
            {
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            }

            sqlBuilder.Append($"{Environment.NewLine} VALUES (");
            allFields.ForEach(p => sqlBuilder.Append($"@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(")");
        }


        /// <summary>
        /// Builds the insert query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildInsertQueryWithIdentityOutputs<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {

            var outputFields = new List<string>() { };
            var members = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor);

                outputFields.AddRange(members.Where(a => a.GetCustomAttribute<SqlColumnAttribute>()?.AutoIncrementBy != null && a.GetCustomAttribute<SqlColumnAttribute>()?.AutoIncrementBy > 0).Select(d => d.Name));
            if (outputFields.IsNullOrEmpty()) throw new MissingIdentityKeyAttributeException(ExceptionHelper.MissingIdentityKeyMessage(typeof(T)));

            var allFields = ObjectToSqlHelper.GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            // Insert sql statement prefix 

            sqlBuilder.Append($"INSERT INTO {tableName} (");

            // Add field names
            allFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}],"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append($") {Environment.NewLine}");

            if (DatabaseType == DataBaseType.SqlServer)
            {
                sqlBuilder.Append($" OUTPUT ");
                if (outputFields.IsNullOrEmpty()) throw new MissingIdentityKeyAttributeException(ExceptionHelper.MissingIdentityKeyMessage(typeof(T)));
                outputFields.ForEach(delegate(string s)
                {
                    sqlBuilder.Append(
                        $" INSERTED.[{members.FirstOrDefault(av => av.Name == s)?.GetNameFromCustomAttributeOrDefault() ?? s}] ,");
                });
            }

            if (!outputFields.IsNullOrEmpty())
            {
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            }

            sqlBuilder.Append($"{Environment.NewLine} VALUES (");
            allFields.ForEach(p => sqlBuilder.Append($"@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(")");
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = ObjectToSqlHelper.GetNonIdentityFields<T>(IncludeNonPublicAccessor);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpdateQuery(StringBuilder sqlBuilder, string tableName,Type type)
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields(IncludeNonPublicAccessor,type);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = ObjectToSqlHelper.GetNonIdentityFields(IncludeNonPublicAccessor,type);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dynamicObject"></param>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpdateQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName) where T : IDynamicMetaObjectProvider
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields<T>(dynamicObject);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = ObjectToSqlHelper.GetNonIdentityFields<T>(dynamicObject);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }



        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        public void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys) where T : class
        {

            var keyFields = new List<MemberWrapper>() { }; 
            if (overrideKeys != null)
            {
                var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
                    keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();
            }
            else
            {
                keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);
            }
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = ObjectToSqlHelper.GetNonIdentityFields<T>(IncludeNonPublicAccessor);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }


        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        public void BuildUpdateQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName, List<string> overrideKeys) where T : IDynamicMetaObjectProvider
        {
            overrideKeys.IsEmptyThrow(nameof(overrideKeys));
            var keyFields = ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => overrideKeys.Contains(m.Name)).AsList();

            var updateFields = ObjectToSqlHelper.GetNonIdentityFields<T>(dynamicObject);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }


        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildDeleteQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);
            
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName} ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildDeleteQuery(StringBuilder sqlBuilder, string tableName, Type type) 
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields(IncludeNonPublicAccessor, type);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName} ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dynamicObject"></param>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildDeleteQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName) where T : IDynamicMetaObjectProvider
        {

            var keyFields =  ObjectToSqlHelper.GetKeyFields<T>(dynamicObject);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName} ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }


        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        public void BuildDeleteQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys ) where T : class
        {

            var keyFields = new List<MemberWrapper>() { };

            if (overrideKeys != null)
            {
                var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
                keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();
            }
            else
            {
                keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);
            }
            if(keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName} ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dynamicObject"></param>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        public void BuildDeleteQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName, List<string> overrideKeys) where T : IDynamicMetaObjectProvider
        {
            overrideKeys.IsEmptyThrow(nameof(overrideKeys));
            var keyFields = ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => overrideKeys.Contains(m.Name)).ToList();
            
            sqlBuilder.Append($"DELETE FROM {tableName} ");
            ObjectToSqlHelper.BuildWhereClause(sqlBuilder, keyFields);
        }




        /// <summary>
        /// Builds the get query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereClause">The where clause.</param>
        public void BuildGetQuery<T>(StringBuilder sqlBuilder, string tableName, string whereClause) where T : class
        {
            sqlBuilder.Append($"SELECT * FROM {tableName ?? typeof(T).Name} ");
            if (string.IsNullOrEmpty(whereClause))
            {

            }
            else
            {
                sqlBuilder.Append(whereClause.ToLower().Replace(" ", "").StartsWith("where") ? $"{whereClause}" : $"WHERE {whereClause}");
            }
        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery<T>(sb, tableName);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);
            var sb2 = new StringBuilder();
            ObjectToSqlHelper.BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }

        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpsertQuery(StringBuilder sqlBuilder, string tableName,Type type) 
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields(IncludeNonPublicAccessor,type);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(sb, tableName,type);
            var sb1 = new StringBuilder();
            BuildInsertQuery(sb1, tableName,type);
            var sb2 = new StringBuilder();
            ObjectToSqlHelper.BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }

        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpsertQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName) where T : IDynamicMetaObjectProvider
        {

            var keyFields = ObjectToSqlHelper.GetKeyFields<T>(dynamicObject);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(dynamicObject, sb, tableName);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(dynamicObject, sb1, tableName);
            var sb2 = new StringBuilder();
            ObjectToSqlHelper.BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }

        public void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys ) where T : class
        {

            var keyFields = new List<MemberWrapper>() { };

            if (overrideKeys != null)
            {
                var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
                keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();
            }
            else
            {
                keyFields = ObjectToSqlHelper.GetKeyFields<T>(IncludeNonPublicAccessor);
            }
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(sb, tableName, overrideKeys);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);
            var sb2 = new StringBuilder();
            ObjectToSqlHelper.BuildWhereClause(sb2,keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}",sb.ToString(),sb1.ToString()));

        }

        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        public void BuildUpsertQuery<T>(T dynamicObject, StringBuilder sqlBuilder, string tableName, List<string> overrideKeys ) where T : IDynamicMetaObjectProvider
        {

            overrideKeys.IsEmptyThrow(nameof(overrideKeys));
            var keyFields = ExtFastMember.GetMemberWrappers<T>(dynamicObject).Where(m => overrideKeys.Contains(m.Name)).ToList();

            var sb = new StringBuilder();
            BuildUpdateQuery(dynamicObject,sb, tableName, overrideKeys);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(dynamicObject,sb1, tableName);
            var sb2 = new StringBuilder();
            ObjectToSqlHelper.BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableAlias"></param>
        /// <param name="memberWrappers"></param>
        /// <param name="sqlSyntax"></param>
        /// <returns>item 1 is the actual columns being selected
        ///          item 2 is the split on column</returns>
        internal Tuple<string, string> BuildSelectColumnStatement(char tableAlias, List<MemberWrapper> memberWrappers, SqlSyntaxHelper sqlSyntax)
        {
            var sb = new StringBuilder();
            var isFirstTime = true;
            var splitOn = "";
            memberWrappers.Where(a1 => a1.GetCustomAttribute<SqlTableAttribute>() == null && a1.GetCustomAttribute<SqlColumnAttribute>()?.Ignore != true).ToList().ForEach(delegate (MemberWrapper member) // BUILD SQL COLUMNS
            {
                var columnName = $"{tableAlias}.{sqlSyntax.GetTableOpenChar()}{member.Name}{sqlSyntax.GetTableClosedChar()}";
                sb.AppendLine($"{columnName} , ");
                if (isFirstTime)
                {
                    splitOn = member.Name;
                    isFirstTime = false;
                }
            });
            return new Tuple<string, string>(sb.ToString(), splitOn);
        }

        internal void BuildJoinOnStatement(List<MemberWrapper> members, char mainTableAlias, List<MemberWrapper> members1, char secondTableAlias, StringBuilder sqlFromBuilder)
        {
            var safeKeyword = " AND ";
            members.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true && a.GetCustomAttribute<SqlTableAttribute>() == null).ToList().ForEach(delegate (MemberWrapper mainTableColumn) // LOOP THRU MAIN TABLE PROPERTIES 
            {

                members1.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true).ToList().ForEach(delegate (MemberWrapper secondTableColumn)
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


    }
}
