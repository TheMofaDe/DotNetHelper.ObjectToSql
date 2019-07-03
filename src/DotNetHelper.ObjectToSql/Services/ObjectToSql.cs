using System;
using System.Collections.Generic;
using System.Data.Common;
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
using DotNetHelper.ObjectToSql.Model;

namespace DotNetHelper.ObjectToSql.Services
{
    public class ObjectToSql
    {

        public bool IncludeNonPublicAccessor { get; set; } = true;
        public DataBaseType DatabaseType { get; }

        public ObjectToSql(DataBaseType type, bool includeNonPublicAccessor)
        {
            DatabaseType = type;
            IncludeNonPublicAccessor = includeNonPublicAccessor;
        }
        public ObjectToSql(DataBaseType type)
        {
            DatabaseType = type;
        }


        private void ThrowIfDynamicOrAnonymous<T>(ActionType actionType) where T : class
        {
            if (typeof(T).IsTypeDynamic()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Dynamic"));
            if (typeof(T).IsTypeAnonymousType()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Anonymous"));
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
        public string BuildQuery<T>(string tableName, ActionType actionType) where T : class
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
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
        public string BuildQuery<T>(string tableName, ActionType actionType, params Expression<Func<T, object>>[] primaryKeys) where T : class
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildInsertQuery<T>(sqlBuilder, tableName);
                    break;
                case ActionType.Update:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildUpdateQuery<T>(sqlBuilder, tableName, primaryKeys);
                    break;
                case ActionType.Upsert:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildUpsertQuery<T>(sqlBuilder, tableName, primaryKeys);
                    break;
                case ActionType.Delete:
                    ThrowIfDynamicOrAnonymous<T>(actionType);
                    BuildDeleteQuery<T>(sqlBuilder, tableName, primaryKeys);
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
        public string BuildQuery(string tableName, ActionType actionType, object instance)
        {
            instance.IsNullThrow(nameof(instance));
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    if (instance.GetType().IsTypeDynamic()) throw new InvalidOperationException(ExceptionHelper.InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(actionType, "Dynamic"));
                    BuildInsertQuery(sqlBuilder, tableName, instance.GetType());
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
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <typeparam name="T">a class</typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <param name="instance">Only required for dynamic types otherwise can be null </param>
        /// <param name="runTimeAttributes"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception>
        public string BuildQuery<T>(string tableName, ActionType actionType, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class
        {

            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQuery(sqlBuilder, tableName, instance, runTimeAttributes);
                    break;
                case ActionType.Update:
                    BuildUpdateQuery(sqlBuilder, tableName, instance, runTimeAttributes);
                    break;
                case ActionType.Upsert:
                    BuildUpsertQuery(sqlBuilder, tableName, instance, runTimeAttributes);
                    break;
                case ActionType.Delete:
                    BuildDeleteQuery(sqlBuilder, tableName, instance, runTimeAttributes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="actionType"></param>
        /// <param name="outputFields"></param>
        /// <returns></returns>
        public string BuildQueryWithOutputs<T>(string tableName, ActionType actionType, params Expression<Func<T, object>>[] outputFields) where T : class
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQueryWithOutputs<T>(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Update:
                    BuildUpdateQueryWithOutputs<T>(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Upsert:
                    BuildUpsertQueryWithOutputs<T>(sqlBuilder, tableName, outputFields);
                    break;
                case ActionType.Delete:
                    BuildDeleteQueryWithOutputs<T>(sqlBuilder, tableName, outputFields);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }


        //public string BuildQueryWithOutputs<T>(string tableName, ActionType actionType) where T : class
        //{
        //    var sqlBuilder = new StringBuilder();
        //    switch (actionType)
        //    {
        //        case ActionType.Insert:
        //            ThrowIfDynamicOrAnonymous<T>(actionType);
        //            BuildInsertQuery<T>(sqlBuilder, tableName);
        //            break;
        //        case ActionType.Update:
        //            ThrowIfDynamicOrAnonymous<T>(actionType);
        //            BuildUpdateQuery<T>(sqlBuilder, tableName);
        //            break;
        //        case ActionType.Upsert:
        //            ThrowIfDynamicOrAnonymous<T>(actionType);
        //            BuildUpsertQuery<T>(sqlBuilder, tableName);
        //            break;
        //        case ActionType.Delete:
        //            ThrowIfDynamicOrAnonymous<T>(actionType);
        //            BuildDeleteQuery<T>(sqlBuilder, tableName);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
        //    }
        //    return sqlBuilder.ToString();
        //}
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
            var allFields = GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} (");

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
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildInsertQuery<T>(StringBuilder sqlBuilder, string tableName, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class
        {
            var allFields = GetNonIdentityFields<T>(runTimeAttributes, instance);
            if (allFields.IsNullOrEmpty()) throw new InvalidOperationException($"The list of {nameof(RunTimeAttributeMap)} didn't contain any matching properties with {nameof(T)} that isn't declared as an identity column");
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} (");

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
        private void BuildInsertQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {
            var allFields = GetNonIdentityFields(IncludeNonPublicAccessor, type);
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? type.GetTableNameFromCustomAttributeOrDefault()} (");

            // Add field names
            allFields.ForEach(delegate (MemberWrapper member)
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
        private void BuildInsertQueryWithOutputs<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] outFields) where T : class
        {

            var outputFields = outFields.GetPropertyNamesFromExpressions();

            outputFields.IsEmptyThrow(nameof(outputFields));
            var allFields = GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            // Insert sql statement prefix 

            sqlBuilder.Append($"INSERT INTO {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} (");

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

            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            var updateFields = GetNonKeyFields<T>(IncludeNonPublicAccessor);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="type"></param>
        private void BuildUpdateQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {

            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            //var updateFields = GetNonIdentityFields(IncludeNonPublicAccessor, type);
            var updateFields = GetNonKeyFields(IncludeNonPublicAccessor, type);

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? type.GetTableNameFromCustomAttributeOrDefault()} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class
        {

            var keyFields = GetKeyFields<T>(runTimeAttributes, instance);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            //var updateFields = GetNonIdentityFields<T>(runTimeAttributes, instance);
            var updateFields = GetNonKeyFields<T>(runTimeAttributes, instance);
            if (updateFields.IsNullOrEmpty()) throw new InvalidOperationException($"The list of {nameof(RunTimeAttributeMap)} didn't contain any matching properties with {nameof(T)} that isn't declared as an key column");
            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, keyFields);
        }



        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="overrideKeys"></param>
        private void BuildUpdateQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys) where T : class
        {
            overrideKeys.IsNullThrow(nameof(overrideKeys));
            overrideKeys.IsEmptyThrow(nameof(overrideKeys));

            var keyFields = new List<MemberWrapper>() { };
            var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
            keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);
            //var updateFields = GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            var updateFields = GetNonKeyFields<T>(IncludeNonPublicAccessor);
            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, keyFields);
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
            outputFields.IsEmptyThrow(nameof(outputFields));

            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            //var updateFields = GetNonIdentityFields<T>(IncludeNonPublicAccessor);
            var updateFields = GetNonKeyFields<T>(IncludeNonPublicAccessor);
            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} SET ");

            // Build Set fields
            updateFields.ForEach(p => sqlBuilder.Append($"[{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma


            var members = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor);
            sqlBuilder.Append($" OUTPUT");
            outputFields.ForEach(delegate (string s) {
                sqlBuilder.Append($" DELETED.[{members.FirstOrDefault(av => av.Name == s)?.GetNameFromCustomAttributeOrDefault() ?? s}] ,");
            });
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);


            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, keyFields);
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
            var keyFields = GetKeyFields<T>(IncludeNonPublicAccessor);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            sqlBuilder.Append($"DELETE FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} ");
            BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildDeleteQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {

            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName ?? type.GetTableNameFromCustomAttributeOrDefault()} ");
            BuildWhereClause(sqlBuilder, keyFields);
        }

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildDeleteQuery<T>(StringBuilder sqlBuilder, string tableName, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class
        {

            var keyFields = GetKeyFields<T>(runTimeAttributes, instance);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} ");
            BuildWhereClause(sqlBuilder, keyFields);
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
            overrideKeys.IsNullThrow(nameof(overrideKeys));
            overrideKeys.IsEmptyThrow(nameof(overrideKeys));
            var keyFields = new List<MemberWrapper>() { };

            var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
            keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();

            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);


            sqlBuilder.Append($"DELETE FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} ");
            BuildWhereClause(sqlBuilder, keyFields);
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

            sqlBuilder.Append($"DELETE FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} ");

            var members = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor);
            sqlBuilder.Append($" OUTPUT");
            outputFields.ForEach(delegate (string s) {
                sqlBuilder.Append($" DELETED.[{members.FirstOrDefault(av => av.Name == s)?.GetNameFromCustomAttributeOrDefault() ?? s}] ,");
            });

            BuildWhereClause(sqlBuilder, keyFields);
        }


        #endregion

        #region UPSERT METHODS


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

            var sb = new StringBuilder();
            BuildUpdateQuery<T>(sb, tableName);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} {sb2}", sb.ToString(), sb1.ToString()));

        }

        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName, params Expression<Func<T, object>>[] overrideKeys) where T : class
        {
            var sb = new StringBuilder();
            BuildUpdateQuery<T>(sb, tableName, overrideKeys);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName);

            var outputFields = overrideKeys.GetPropertyNamesFromExpressions();
            var keyFields = ExtFastMember.GetMemberWrappers<T>(IncludeNonPublicAccessor).Where(m => outputFields.Contains(m.Name)).ToList();
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName ?? typeof(T).GetTableNameFromCustomAttributeOrDefault()} {sb2}", sb.ToString(), sb1.ToString()));

        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery(StringBuilder sqlBuilder, string tableName, Type type)
        {

            var keyFields = GetKeyFields(IncludeNonPublicAccessor, type);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(sb, tableName, type);
            var sb1 = new StringBuilder();
            BuildInsertQuery(sb1, tableName, type);
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery<T>(StringBuilder sqlBuilder, string tableName, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class
        {

            var keyFields = GetKeyFields<T>(runTimeAttributes, instance);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery<T>(sb, tableName, instance, runTimeAttributes);
            var sb1 = new StringBuilder();
            BuildInsertQuery<T>(sb1, tableName, instance, runTimeAttributes);
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

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
            BuildUpdateQueryWithOutputs<T>(sb, tableName, outFields);
            var sb1 = new StringBuilder();
            BuildInsertQueryWithOutputs<T>(sb, tableName, outFields);
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, keyFields);
            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));
        }

        #endregion

        #region GetNonKeyFields

        /// <summary>
        /// Gets the non key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields<T>(bool includeNonPublicAccessor) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => !m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).AsList();
        }
        /// <summary>
        /// Gets the non key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonKeyFields<T>(List<RunTimeAttributeMap> runTimeAttributes, T instance) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            if (instance is IDynamicMetaObjectProvider a)
            {
                //return ExtFastMember.GetMemberWrappers(a).Where(m => runTimeAttributes.FirstOrDefault(r => !m.IsMemberAPrimaryKeyColumn() && r.PropertyName == m.Name) != null).AsList();
                return ExtFastMember.GetMemberWrappers(a).Where(m => runTimeAttributes.FirstOrDefault(r => !r.IsMemberAPrimaryKeyColumn() && r.PropertyName == m.Name) != null).AsList();
            }
            return ExtFastMember.GetMemberWrappers<T>(true).Where(m => runTimeAttributes.FirstOrDefault(r => !r.IsMemberAPrimaryKeyColumn() && r.PropertyName == m.Name) != null).AsList();
        }

        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
            return ExtFastMember.GetMemberWrappers<T>(includeNonPublicAccessor).Where(m => m.IsMemberAPrimaryKeyColumn() && !m.ShouldMemberBeIgnored()).ToList();
        }

        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetKeyFields<T>(List<RunTimeAttributeMap> runTimeAttributes, T instance) where T : class
        {
            // Get the primary key fields - The properties in the class decorated with PrimaryKey attribute.
            if (instance is IDynamicMetaObjectProvider a)
            {
                return ExtFastMember.GetMemberWrappers(a).Where(m => runTimeAttributes.FirstOrDefault(r => r.IsMemberAPrimaryKeyColumn() && !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) != null).AsList();
            }
            return ExtFastMember.GetMemberWrappers<T>(true).Where(m => runTimeAttributes.FirstOrDefault(r => r.IsMemberAPrimaryKeyColumn() && !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) != null).AsList();
        }

        /// <summary>
        /// Gets the key fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonIdentityFields<T>(List<RunTimeAttributeMap> runTimeAttributes, T instance) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            if (instance is IDynamicMetaObjectProvider a)
            {
                return ExtFastMember.GetMemberWrappers(a).Where(m => runTimeAttributes.FirstOrDefault(r => r.IsMemberAnIdentityColumn() && !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) == null).AsList();
            }
            return ExtFastMember.GetMemberWrappers<T>(true).Where(m => runTimeAttributes.FirstOrDefault(r => !r.IsMemberAnIdentityColumn() && !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) != null).AsList();
        }

        /// <summary>
        /// Gets the non identity fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;MemberWrapper&gt;.</returns>
        public List<MemberWrapper> GetNonIdentityFields(bool includeNonPublicAccessor, Type type)
        {
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
        public List<MemberWrapper> GetAllNonIgnoreFields<T>(List<RunTimeAttributeMap> runTimeAttributes, T instance) where T : class
        {
            // Get non primary key fields - the ones we want to update.
            if (instance is IDynamicMetaObjectProvider a)
            {
                return ExtFastMember.GetMemberWrappers(a).Where(m => runTimeAttributes.FirstOrDefault(r => !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) != null).AsList();
            }
            return ExtFastMember.GetMemberWrappers<T>(true).Where(m => runTimeAttributes.FirstOrDefault(r => !r.ShouldMemberBeIgnored() && r.PropertyName == m.Name) != null).AsList();
        }


        /// <summary>
        /// Gets all non ignore fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// Converts to database value.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public object ConvertToDatabaseValue(MemberWrapper member, object value, Func<object, string> XmlSerializer, Func<object, string> JsonSerializer, Func<object, string> CsvSerializer)
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
                        XmlSerializer.IsNullThrow(nameof(XmlSerializer), new ArgumentNullException(nameof(XmlSerializer), $"{ExceptionHelper.NullSerializer(member, SerializableType.XML)}"));
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
        /// Converts to database value.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public object ConvertToDatabaseValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (value is DateTime time && time == DateTime.MinValue) // TODO :: VALID NULLABLE DATETIME FALL IN THIS METHOD AS WELL
            {
                return new DateTime(1753, 01, 01);
            }
            return value;
        }



        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco">The poco.</param>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<DbParameter> BuildDbParameterList<T>(T instance, Func<string, object, DbParameter> GetNewParameter, Func<object, string> XmlSerializer, Func<object, string> JsonSerializer, Func<object, string> CsvSerializer) where T : class
        {
            var list = new List<DbParameter>() { };
            List<MemberWrapper> members;
            if (instance is IDynamicMetaObjectProvider a)
            {
                members = GetAllNonIgnoreFields(new List<RunTimeAttributeMap>() { }, instance); // create a dynamic overload with imeta T
            }
            else
            {
                members = GetAllNonIgnoreFields<T>(IncludeNonPublicAccessor);
            }
            members.ForEach(delegate (MemberWrapper p)
            {
                var parameterValue = ConvertToDatabaseValue(p, p.GetValue(instance), XmlSerializer, JsonSerializer, CsvSerializer);
                // try
                // {
                list.Add(GetNewParameter($"@{p.Name}", parameterValue));
                // }
                // catch (ArgumentException error)
                // {
                //     // TODO :: Throw custom exception explain that we couldn't find a mapping sqldbtype for this value type . In most case this is a list
                // TODO  :: THE SOLUTION SHOULD BE TO Apply A serailization attribute on as you can't store list in a database 
                // }
            });
            return list;
        }

        #endregion

        /// <summary>
        /// Builds the where clause.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="keyFields">The key fields.</param>

        public void BuildWhereClause(StringBuilder sqlBuilder, List<MemberWrapper> keyFields)
        {
            if (keyFields.IsNullOrEmpty())
            {
            }
            else
            {
                sqlBuilder.Append("WHERE");
                keyFields.ForEach(p => sqlBuilder.Append($" [{p.GetNameFromCustomAttributeOrDefault()}]=@{p.Name} AND"));
                if (sqlBuilder.ToString().EndsWith(" AND"))
                    sqlBuilder.Remove(sqlBuilder.Length - 4, 4); // Remove the last AND       
            }

        }

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
        //    memberWrappers.Where(a1 => a1.GetCustomAttribute<DBTableAttribute>() == null && a1.GetCustomAttribute<SqlColumnAttribute>()?.Ignore != true).ToList().ForEach(delegate (MemberWrapper member) // BUILD SQL COLUMNS
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
        //    members.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true && a.GetCustomAttribute<DBTableAttribute>() == null).ToList().ForEach(delegate (MemberWrapper mainTableColumn) // LOOP THRU MAIN TABLE PROPERTIES 
        //    {

        //        members1.Where(a => !a.GetCustomAttribute<SqlColumnAttribute>()?.MappingIds.IsNullOrEmpty() != true).ToList().ForEach(delegate (MemberWrapper secondTableColumn)
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