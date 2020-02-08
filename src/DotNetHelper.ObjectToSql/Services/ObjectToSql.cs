using DotNetHelper.FastMember.Extension;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace DotNetHelper.ObjectToSql.Services
{
    public class ObjectToSql
    {


        public bool IncludeNonPublicAccessor { get; set; } = true;
        public DataBaseType DatabaseType { get; }
        public SqlSyntaxHelper SqlSyntaxHelper { get; }

        public ObjectToSql(DataBaseType type, bool includeNonPublicAccessor)
        {
            DatabaseType = type;
            IncludeNonPublicAccessor = includeNonPublicAccessor;
            SqlSyntaxHelper = new SqlSyntaxHelper(type);
        }
        public ObjectToSql(DataBaseType type)
        {
            DatabaseType = type;
            SqlSyntaxHelper = new SqlSyntaxHelper(type);
        }


        private void ThrowIfDynamicOrAnonymous<T>(ActionType actionType) where T : class
        {
            if (typeof(T).IsTypeDynamic()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Dynamic"));
            if (typeof(T).IsTypeAnonymousType()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Anonymous"));
        }
        private void ThrowIfDynamic<T>(ActionType actionType, Type type) where T : class
        {
            if (typeof(T).IsTypeDynamic()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, type.Name));
        }
        private void ThrowIfDynamicOrAnonymous(ActionType actionType, Type type)
        {
            if (type.IsTypeDynamic()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Dynamic"));
            if (type.IsTypeAnonymousType()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Anonymous"));
        }




        #region Public Method Build Query




        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <typeparam name="T">a class</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQuery<T>(ActionType actionType, string tableName = null) where T : class
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    ThrowIfDynamic<T>(actionType, typeof(T));
                    BuildInsertQuery<T>(sqlBuilder, tableName);
                    break;
                case ActionType.Update:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildUpdateQuery<T>(sqlBuilder, tableName);
                    break;
                case ActionType.Upsert:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildUpsertQuery<T>(sqlBuilder, tableName);
                    break;
                case ActionType.Delete:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildDeleteQuery<T>(sqlBuilder, tableName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <typeparam name="T">a class</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <param name="primaryKeys">During the query build process the properties in the expression will be treated as primary keys</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQuery<T>(ActionType actionType, string tableName = null, params Expression<Func<T, object>>[] primaryKeys) where T : class
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQuery<T>(sqlBuilder, tableName);
                    break;
                case ActionType.Update:
                    BuildUpdateQuery(sqlBuilder, tableName, primaryKeys);
                    break;
                case ActionType.Upsert:
                    BuildUpsertQuery(sqlBuilder, tableName, primaryKeys);
                    break;
                case ActionType.Delete:
                    BuildDeleteQuery(sqlBuilder, tableName, primaryKeys);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <param name="instance">Only required for dynamic types otherwise can be null </param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception>
        public string BuildQuery(ActionType actionType, object instance, string tableName = null)
        {
            instance.IsNullThrow(nameof(instance));
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQuery(sqlBuilder, tableName, instance.GetType(), instance);
                    break;
                case ActionType.Update:
                    ThrowIfDynamicOrAnonymous(actionType, instance.GetType());
                    BuildUpdateQuery(sqlBuilder, tableName, instance.GetType());
                    break;
                case ActionType.Upsert:
                    ThrowIfDynamicOrAnonymous(actionType, instance.GetType());
                    BuildUpsertQuery(sqlBuilder, tableName, instance.GetType());
                    break;
                case ActionType.Delete:
                    ThrowIfDynamicOrAnonymous(actionType, instance.GetType());
                    BuildDeleteQuery(sqlBuilder, tableName, instance.GetType());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }



        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionType"></param>
        /// <param name="outputFields"></param>
        /// <returns></returns>
        public string BuildQueryWithOutputs<T>(ActionType actionType, params Expression<Func<T, object>>[] outputFields) where T : class
        {
            return BuildQueryWithOutputs(actionType, null, outputFields);
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="actionType"></param>
        /// <param name="outputFields"></param>
        /// <returns></returns>
        public string BuildQueryWithOutputs<T>(ActionType actionType, string tableName = null, params Expression<Func<T, object>>[] outputFields) where T : class
        {
            if (DatabaseType == DataBaseType.Sqlite || DatabaseType == DataBaseType.MySql) throw new NotImplementedException($"BuildQueryWithOutputs is not supported by {DatabaseType}");
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQueryWithOutputs(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Update:
                    BuildUpdateQueryWithOutputs(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Upsert:
                    BuildUpsertQueryWithOutputs(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Delete:
                    BuildDeleteQueryWithOutputs(sqlBuilder, tableName, outputFields);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }




        #endregion

        #region INSERT METHODS





        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildInsertQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {
            BuildInsertQuery(sqlBuilder, tableName, typeof(T));
        }


        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type"></param>
        private void BuildInsertQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {
            // ReSharper disable once IntroduceOptionalParameters.Local
            BuildInsertQuery(sqlBuilder, tableName, type, null);
        }

        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type">type that is used to generate insert statement</param>
        /// <param name="instance"></param>
        private void BuildInsertQuery(StringBuilder sqlBuilder, string tableName, Type type, object instance)
        {


            var allFields = instance == null ?
                  GetNonIdentityFields(IncludeNonPublicAccessor, type)
                : GetNonIdentityFields(IncludeNonPublicAccessor, type, instance);

            var columns = allFields.Where(m => !m.IsMemberIgnoredForInsertSql()).Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            // This uses the .net property name & ignores any attribute mapto name to ensure duplication is prevented
            var valueColumns = allFields.Where(m => !m.IsMemberIgnoredForInsertSql()).Select(c => c.Name).AsList();
            sqlBuilder.Append(SqlGenerator.BuildInsertQuery(SqlSyntaxHelper, tableName.GetTableName(DatabaseType, type), columns, valueColumns));
        }


        /// <summary>
        /// Builds the insert query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="outFields">array of properties you wish to get the inserted values of (Only Support SQL Server)</param>
        private void BuildInsertQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {

            var outputFields = outFields.GetPropertyNamesFromExpressions();
            outputFields.IsEmptyThrow(nameof(outputFields));

            var allFields = GetNonIdentityFields<T>(IncludeNonPublicAccessor).Where(m => !m.IsMemberIgnoredForInsertSql()).AsList();
            var columns = allFields.Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            var valueColumns = allFields.Select(c => c.Name).AsList();

            sqlBuilder.Append(SqlGenerator.BuildInsertIntoTable(SqlSyntaxHelper, tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()));
            sqlBuilder.Append($" {SqlGenerator.BuildColumnsInParentheses(SqlSyntaxHelper, columns)}");
            sqlBuilder.Append($" {SqlGenerator.BuildOutputFields<T>(SqlSyntaxHelper, outputFields, OutputType.INSERTED)}");
            sqlBuilder.Append($"{Environment.NewLine} {SqlGenerator.BuildValues(SqlSyntaxHelper, valueColumns)}");
        }




        #endregion

        #region UPDATE METHODS



        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {
            BuildUpdateQuery(sqlBuilder, tableName, typeof(T));
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type"></param>
        private void BuildUpdateQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {
            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = GetNonKeyFields(IncludeNonPublicAccessor, type);
            var columns = updateFields.Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            var valueColumns = updateFields.Select(c => c.Name).AsList();

            sqlBuilder.Append($"{SqlGenerator.BuildUpdateTable(tableName.GetTableName(DatabaseType, type))} ");
            sqlBuilder.Append($"{SqlGenerator.BuildSetColumns(SqlSyntaxHelper, columns, valueColumns)} ");
            sqlBuilder.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");

        }


        /// <summary>
        /// Builds the update query. using the specified primaryKeys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="primaryKeys">the fields to include in the where clause</param>
        private void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] primaryKeys) where T : class
        {

            var outputFields = primaryKeys.GetPropertyNamesFromExpressions();
            var allFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor);
            var keyFields = allFields.Where(m => outputFields.Contains(m.Name)).AsList();
            var nonKeyFields = allFields.Where(m => !outputFields.Contains(m.Name)).AsList();

            if (outputFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var columns = nonKeyFields.Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            var valueColumns = nonKeyFields.Select(c => c.Name).AsList();

            sqlBuilder.Append($"{SqlGenerator.BuildUpdateTable(tableName.GetTableName(DatabaseType, typeof(T)))} ");
            sqlBuilder.Append($"{SqlGenerator.BuildSetColumns(SqlSyntaxHelper, columns, valueColumns)} ");
            sqlBuilder.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");
        }


        /// <summary>
        /// Builds the update query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="outFields"></param>
        private void BuildUpdateQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {

            var outputFields = outFields.GetPropertyNamesFromExpressions();
            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            var nonKeyFields = GetNonKeyFields<T>(IncludeNonPublicAccessor);

            // if (outputFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var columns = nonKeyFields.Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            var valueColumns = nonKeyFields.Select(c => c.Name).AsList();

            sqlBuilder.Append($"{SqlGenerator.BuildUpdateTable(tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault())} ");
            sqlBuilder.Append($"{SqlGenerator.BuildSetColumns(SqlSyntaxHelper, columns, valueColumns)} ");
            sqlBuilder.Append($"{SqlGenerator.BuildOutputFields<T>(SqlSyntaxHelper, outputFields, OutputType.DELETED)}");
            sqlBuilder.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");




        }




        #endregion

        #region  DELETE METHODS




        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildDeleteQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {
            BuildDeleteQuery(sqlBuilder, tableName, typeof(T));
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type"></param>
        private void BuildDeleteQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {

            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            sqlBuilder.Append(SqlGenerator.BuildDeleteQuery(SqlSyntaxHelper, tableName.GetTableName(DatabaseType, type), keyFields));
        }



        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        private void BuildDeleteQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys) where T : class
        {
            var primaryKeys = overrideKeys.GetPropertyNamesFromExpressions();
            if (primaryKeys.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            sqlBuilder.Append($"{SqlGenerator.BuildDeleteFromTable(tableName.GetTableName(DatabaseType, typeof(T)))} ");
            var whereClauseFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => primaryKeys.Contains(m.Name)).AsList();

            sqlBuilder.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, whereClauseFields)}");
        }

        /// <summary>
        /// Builds the delete query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="outFields"></param>
        private void BuildDeleteQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {
            var outputFields = outFields.GetPropertyNamesFromExpressions();
            outputFields.IsEmptyThrow(nameof(outputFields));

            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            sqlBuilder.Append($"{SqlGenerator.BuildDeleteFromTable(tableName.GetTableName(DatabaseType, typeof(T)))} ");
            sqlBuilder.Append($"{SqlGenerator.BuildOutputFields<T>(SqlSyntaxHelper, outputFields, OutputType.DELETED)}");
            sqlBuilder.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");
        }


        #endregion

        #region UPSERT METHODS


        private void SqLiteBuildUpsertQuery<T>(StringBuilder sqlBuilder, List<MemberWrapper> keyFields, string tableName, string whereClause, string normalInsertSQl) where T : class
        {
            var updateFields = GetNonKeyFields<T>(IncludeNonPublicAccessor);

            var trueForAll = keyFields.TrueForAll(w => (w.Type == typeof(int) || w.Type == typeof(long))); // THESE ARE TREATED LKE IDENTITY FIELDS IF NOT SPECIFIED https://www.sqlite.org/autoinc.html
            if (trueForAll)
            {
                sqlBuilder.Append($@"{SqlGenerator.BuildInsertOrReplace(tableName)} ({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))},{string.Join(",", updateFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))}) VALUES ( {string.Join(",", keyFields.Select(w => $"(SELECT {w.GetNameFromCustomAttributeOrDefault()} FROM {tableName} {whereClause})"))}, {string.Join(",", updateFields.Select(w => $"@{w.Name}"))} )");
            }
            else
            {
                sqlBuilder.Append($"{normalInsertSQl} ON CONFLICT ({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))} DO UPDATE SET ");

                // Build Set fields
                updateFields.ForEach(p => sqlBuilder.Append($"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{p.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}=@{p.Name},"));
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

                // Build Where clause.
                sqlBuilder.Append($" {whereClause}");
            }
        }

        private void SqLiteBuildUpsertQuery(StringBuilder sqlBuilder, List<MemberWrapper> keyFields, string tableName, string whereClause, string normalInsertSQl, Type type)
        {
            var updateFields = GetNonKeyFields(IncludeNonPublicAccessor, type);

            var trueForAll = keyFields.TrueForAll(w => (w.Type == typeof(int) || w.Type == typeof(long))); // THESE ARE TREATED LKE IDENTITY FIELDS IF NOT SPECIFIED https://www.sqlite.org/autoinc.html
            if (trueForAll)
            {
                sqlBuilder.Append($@"{SqlGenerator.BuildInsertOrReplace(tableName)} ");
                //    sqlBuilder.Append($@"{SqlGenerator.BuildColumnsInParentheses(SqlSyntaxHelper, keyFields.Select(w => w.GetNameFromCustomAttributeOrDefault()).AsList())} ");
                sqlBuilder.Append($@"({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))},{string.Join(",", updateFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))}) VALUES ({string.Join(",", keyFields.Select(w => $"(SELECT {w.GetNameFromCustomAttributeOrDefault()} FROM {tableName} {whereClause})"))}, {string.Join(",", updateFields.Select(w => $"@{w.Name}"))} )");
            }
            else
            {
                sqlBuilder.Append($"{normalInsertSQl} ON CONFLICT ({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))} DO UPDATE SET ");

                // Build Set fields
                updateFields.ForEach(p => sqlBuilder.Append($"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{p.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}=@{p.Name},"));
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

                // Build Where clause.
                sqlBuilder.Append($" {whereClause}");
            }
        }


        private void MySqlBuildOnDuplicateKeyUpdate(StringBuilder sqlBuilder, Type type)
        {
            var updateFields = GetNonKeyFields(IncludeNonPublicAccessor, type);
            sqlBuilder.Append($" {SqlSyntaxHelper.ConstMySqlOnDupeKeyUpdate} ");
            updateFields.ForEach(p => sqlBuilder.Append($"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{p.GetNameFromCustomAttributeOrDefault()}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName) where T : class
        {
            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);
            var sb2 = new StringBuilder();
            sb2.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");

            if (DatabaseType == DataBaseType.Sqlite)
            {
                tableName = tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault();
                SqLiteBuildUpsertQuery<T>(sqlBuilder, keyFields, tableName, sb2.ToString(), sb1.ToString());
            }
            else if (DatabaseType == DataBaseType.MySql)
            {
                sqlBuilder.Append(sb1);
                MySqlBuildOnDuplicateKeyUpdate(sqlBuilder, typeof(T));
            }
            else
            {
                var sb = new StringBuilder();
                BuildUpdateQuery<T>(sb, tableName);
                sqlBuilder.Append(SqlSyntaxHelper.BuildIfExistStatement($"{SqlSyntaxHelper.ConstSqlServerSelectTop1} {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} {sb2}", sb.ToString(), sb1.ToString()));
            }
        }

        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys) where T : class
        {
            var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
            var keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).AsList();

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);
            var sb2 = new StringBuilder();
            sb2.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");

            if (DatabaseType == DataBaseType.Sqlite)
            {
                tableName = tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault();
                SqLiteBuildUpsertQuery<T>(sqlBuilder, keyFields, tableName, sb2.ToString(), sb1.ToString());
            }
            else if (DatabaseType == DataBaseType.MySql)
            {
                sqlBuilder.Append(sb1);
                MySqlBuildOnDuplicateKeyUpdate(sqlBuilder, typeof(T));
            }
            else
            {
                var sb = new StringBuilder();
                BuildUpdateQuery(sb, tableName, overrideKeys);
                sqlBuilder.Append(SqlSyntaxHelper.BuildIfExistStatement($"{SqlSyntaxHelper.ConstSqlServerSelectTop1} {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} {sb2}", sb.ToString(), sb1.ToString()));
            }
        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type">relection is done on this type to generate sql</param>
        private void BuildUpsertQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {
            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            tableName = tableName ?? type.GetTableNameFromCustomAttributeOrDefault();

            var sb1 = new StringBuilder();
            BuildInsertQuery(sb1, tableName, type);
            var sb2 = new StringBuilder();
            sb2.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");

            if (DatabaseType == DataBaseType.Sqlite)
            {
                SqLiteBuildUpsertQuery(sqlBuilder, keyFields, tableName, sb2.ToString(), sb1.ToString(), type);
            }
            else if (DatabaseType == DataBaseType.MySql)
            {
                sqlBuilder.Append(sb1);
                MySqlBuildOnDuplicateKeyUpdate(sqlBuilder, type);
            }
            else
            {
                var sb = new StringBuilder();
                BuildUpdateQuery(sb, tableName, type);
                sqlBuilder.Append(SqlSyntaxHelper.BuildIfExistStatement($"{SqlSyntaxHelper.ConstSqlServerSelectTop1} {tableName ?? type.GetTableNameFromCustomAttributeOrDefault()} {sb2}", sb.ToString(), sb1.ToString()));
            }
        }




        /// <summary>
        /// Builds the update query and return the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="outFields"></param>
        private void BuildUpsertQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {
            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQueryWithOutputs(sb, tableName, outFields);
            var sb1 = new StringBuilder();
            BuildInsertQueryWithOutputs(sb, tableName, outFields);
            var sb2 = new StringBuilder();
            sb2.Append($"{SqlGenerator.BuildWhereClauseFromMembers(SqlSyntaxHelper, keyFields)}");
            sqlBuilder.Append(SqlSyntaxHelper.BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));
        }

        #endregion

        #region GetNonKeyFields

        /// <summary>
        /// Gets the non primary key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields<T>(bool includeNonPublicAccessor) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }


        /// <summary>
        /// Gets the non primary key fields.
        /// </summary>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields(bool includeNonPublicAccessor, Type type)
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            return ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => !m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }

        #endregion

        #region GetKeyFields

        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetKeyFields<T>(bool includeNonPublicAccessor) where T : class
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }



        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetKeyFields(bool includeNonPublicAccessor, Type type)
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            return ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }

        #endregion

        #region GetNonIdentityFields 
        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonIdentityFields<T>(bool includeNonPublicAccessor) where T : class
        {
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }


        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonIdentityFields(bool includeNonPublicAccessor, Type type)
        {
            return ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }

        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonIdentityFields(bool includeNonPublicAccessor, Type type, object instance)
        {
            if (instance != null && instance is IDynamicMetaObjectProvider a)
            {
                return ExtFastMember.GetMemberWrappers(a);
            }
            return ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => !m.IsMemberAnIdentityColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }


        #endregion

        #region GetNonIgnoreFields
        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetAllNonIgnoreFields<T>(bool includeNonPublicAccessor) where T : class
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
        public List<MemberWrapper> GetAllNonIgnoreFields<T>(T instance, bool includeNonPublicAccessor) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            if (instance is IDynamicMetaObjectProvider a)
            {
                return ExtFastMember.GetMemberWrappers(a);
            }
            return GetAllNonIgnoreFields(instance.GetType(), includeNonPublicAccessor);
        }


        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetAllNonIgnoreFields(Type type, bool includeNonPublicAccessor)
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            var temp = ExtFastMember.GetMemberWrappers(type, includeNonPublicAccessor).Where(m => !m.ShouldMemberBeIgnored()).AsList();
            return temp;
        }

        #endregion

        #region DB PARAMETERS

        /// <summary>
        /// Converts to database value. If value is null convert it to DBNull.Value or if property is decorated with a serializable attribute
        /// then convert the value to its serialize self
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <param name="csvSerializer"></param>
        /// <param name="xmlSerializer"></param>
        /// <returns>System.Object.</returns>
        public object ConvertToDatabaseValue(MemberWrapper member, object value, Func<object, string> xmlSerializer, Func<object, string> jsonSerializer, Func<object, string> csvSerializer)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            //if (member.Type == typeof(DateTime) && (DateTime)value == DateTime.MinValue || member.Type == typeof(DateTime?) && (DateTime)value == DateTime.MinValue)
            //{
            //    if(DatabaseType == DataBaseType.SqlServer)
            //        return new DateTime(1753, 01, 01);
            //}
            if (member.GetCustomAttribute<SqlColumnAttribute>()?.SerializableType != SerializableType.None)
            {
                switch (member.GetCustomAttribute<SqlColumnAttribute>()?.SerializableType)
                {
                    case SerializableType.Xml:
                        xmlSerializer.IsNullThrow(nameof(xmlSerializer), new ArgumentNullException(nameof(xmlSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.Xml)}"));
                        return xmlSerializer.Invoke(value);
                    case SerializableType.Json:
                        jsonSerializer.IsNullThrow(nameof(jsonSerializer), new ArgumentNullException(nameof(jsonSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.Json)}"));
                        return jsonSerializer.Invoke(value);
                    case SerializableType.Csv:
                        csvSerializer.IsNullThrow(nameof(csvSerializer), new ArgumentNullException(nameof(csvSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.Csv)}"));
                        return csvSerializer.Invoke(value);
                    case SerializableType.None:
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
        /// <param name="instance"></param>
        /// <param name="getNewParameter"></param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<DbParameter> BuildDbParameterList<T>(T instance, Func<string, object, DbParameter> getNewParameter, Func<object, string> xmlSerializer, Func<object, string> jsonSerializer, Func<object, string> csvSerializer) where T : class
        {
            var list = new List<DbParameter>() { };
            var members = GetAllNonIgnoreFields(instance, IncludeNonPublicAccessor);
            members.ForEach(delegate (MemberWrapper p)
            {
                var parameterValue = ConvertToDatabaseValue(p, p.GetValue(instance), xmlSerializer, jsonSerializer, csvSerializer);
                list.Add(getNewParameter($"@{p.Name}", parameterValue));
            });
            return list;
        }

        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="getNewParameter"></param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<DbParameter> BuildDbParameterList<T>(T instance, Func<string, object, DbParameter> getNewParameter) where T : class
        {
            return BuildDbParameterList(instance, getNewParameter, null, null, null);
        }


        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="getNewParameter"></param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<IDbDataParameter> BuildDbDataParameterList<T>(T instance, Func<string, object, IDbDataParameter> getNewParameter, Func<object, string> xmlSerializer, Func<object, string> jsonSerializer, Func<object, string> csvSerializer) where T : class
        {
            var list = new List<IDbDataParameter>() { };
            List<MemberWrapper> members;
            if (instance is IDynamicMetaObjectProvider a)
            {
                members = GetAllNonIgnoreFields(a, IncludeNonPublicAccessor);
            }
            else
            {
                members = GetAllNonIgnoreFields<T>(IncludeNonPublicAccessor);
            }
            members.ForEach(delegate (MemberWrapper p)
            {
                var parameterValue = ConvertToDatabaseValue(p, p.GetValue(instance), xmlSerializer, jsonSerializer, csvSerializer);

                list.Add(getNewParameter($"@{p.Name}", parameterValue));

            });
            return list;
        }

        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="getNewParameter"></param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<IDbDataParameter> BuildDbDataParameterList<T>(T instance, Func<string, object, IDbDataParameter> getNewParameter) where T : class
        {
            return BuildDbDataParameterList(instance, getNewParameter, null, null, null);
        }
        #endregion



        // TODO :: COME BACK WHEN READY 
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tableAlias"></param>
        ///// <param name="memberWrappers"></param>
        ///// <param name="sqlSyntax"></param>
        ///// <returns>item 1 is the actual columns being selected
        /////          item 2 is the split on column</returns>
        //internal Tuple<string, string> BuildSelectColumnStatement(char tableAlias, List<MemberWrapper> memberWrappers, SqlSyntaxHelper sqlSyntax)
        //{
        //    var sb = new StringBuilder();
        //    var isFirstTime = true;
        //    var splitOn = "";
        //    memberWrappers.Where(a1 => a1.GetCustomAttribute<DBTableAttribute>() == null && a1.GetCustomAttribute<SqlColumnAttribute>()?.Ignore != true).AsList().ForEach(delegate (MemberWrapper member) // BUILD SQL COLUMNS
        //    {
        //        var columnName = $"{tableAlias}.{sqlSyntax.GetKeywordEscapeOpenChar()}{member.Name}{sqlSyntax.GetKeywordEscapeClosedChar()}";
        //        sb.AppendLine($"{columnName} , ");
        //        if (isFirstTime)
        //        {
        //            splitOn = member.Name;
        //            isFirstTime = false;
        //        }
        //    });
        //    return new Tuple<string, string>(sb.ToString(), splitOn);
        //}

        //internal void BuildJoinOnStatement(List<MemberWrapper> members, char mainTableAlias, List<MemberWrapper> members1, char secondTableAlias, StringBuilder sqlFromBuilder)
        //{
        //    var safeKeyword = " AND ";
        //    members.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true && a.GetCustomAttribute<DBTableAttribute>() == null).AsList().ForEach(delegate (MemberWrapper mainTableColumn) // LOOP THRU MAIN TABLE PROPERTIES 
        //    {

        //        members1.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true).AsList().ForEach(delegate (MemberWrapper secondTableColumn)
        //        {

        //            var attr = mainTableColumn.GetCustomAttribute<SqlColumnAttribute>();
        //            var attr2 = mainTableColumn.GetCustomAttribute<SqlColumnAttribute>();
        //            if (attr?.MappingIds.ContainAnySameItem(attr2?.MappingIds) == true)
        //            {


        //                sqlFromBuilder.Append($" {mainTableAlias}.{mainTableColumn.Name} " +
        //                                      $"= {secondTableAlias}.{secondTableColumn.Name} ");
        //                sqlFromBuilder.Append(safeKeyword);

        //                // var iHateThis = sqlFromBuilder.ToString().ReplaceLastOccurrance(safeKeyword, string.Empty, StringComparison.Ordinal);
        //                // sqlFromBuilder.Clear();
        //                //  sqlFromBuilder.Append(sqlFromBuilder.ToString().ReplaceLastOccurrance(safeKeyword, string.Empty, StringComparison.Ordinal));
        //                // sqlFromBuilder.Clear();
        //            }
        //        });

        //    });

        //    sqlFromBuilder = sqlFromBuilder.ReplaceLastOccurrence(safeKeyword, string.Empty, StringComparison.Ordinal);

        //}


    }
}