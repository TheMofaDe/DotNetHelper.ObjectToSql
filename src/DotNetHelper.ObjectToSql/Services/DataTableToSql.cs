using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Exceptions;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql.Services
{
    public class DataTableToSql
    {

        public DataBaseType DatabaseType { get; }


        public DataTableToSql(DataBaseType type)
        {
            DatabaseType = type;
        }

        #region Public Method Build Query

        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQuery(DataTable dataTable, ActionType actionType)
        {
            return BuildQuery(dataTable, actionType,dataTable.TableName);
        }


        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQuery(DataTable dataTable, ActionType actionType, string tableName)
        {
            var sqlBuilder = new StringBuilder();
            switch (actionType)
            {
                case ActionType.Insert:
                    BuildInsertQuery(sqlBuilder, dataTable,tableName);
                    break;
                case ActionType.Update:
                    BuildUpdateQuery(sqlBuilder, dataTable, tableName);
                    break;
                case ActionType.Upsert:
                    BuildUpsertQuery(sqlBuilder, dataTable, tableName);
                    break;
                case ActionType.Delete:
                    BuildDeleteQuery(sqlBuilder, dataTable, tableName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            return sqlBuilder.ToString();
        }


        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="dataRow"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQueryFromRowState(DataRow dataRow)
        {
            return BuildQueryFromRowState(dataRow, dataRow.Table.TableName);
        }


        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tableName">Name of the table.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQueryFromRowState(DataRow dataRow,  string tableName )
        {
            var sqlBuilder = new StringBuilder();
            switch (dataRow.RowState)
            {
                case DataRowState.Added:
                    BuildInsertQuery(sqlBuilder, dataRow.Table, tableName);
                    break;
                case DataRowState.Deleted:
                    BuildDeleteQuery(sqlBuilder, dataRow.Table, tableName);
                    break;
                case DataRowState.Detached:
                    break;
                case DataRowState.Modified:
                    BuildUpdateQuery(sqlBuilder, dataRow.Table, tableName);
                    break;
                case DataRowState.Unchanged:
                    BuildUpdateQuery(sqlBuilder, dataRow.Table, tableName);
                    break;
                default:
                    break;
            }
            return sqlBuilder.ToString();
        }


        #endregion



        private (List<string> keyFields, List<string> identityFields, List<string> nonIdentityFields)  GetFields (DataTable dataTable)
        {
            var identityFields = new List<string>(){};
            var keyFields = new List<string>(){};
            var nonIdentityFields = new List<string>() { };

            void AddIdentityOrNot(DataColumn column)
            {
                if (column.AutoIncrement)
                {
                    identityFields.Add(column.ColumnName);
                }
                else
                {
                    nonIdentityFields.Add(column.ColumnName);
                }
            }

            foreach (DataColumn column in dataTable.Columns)
            {
                if (dataTable.PrimaryKey.Contains(column))
                {
                    keyFields.Add(column.ColumnName);
                    AddIdentityOrNot(column);
                }
                else
                {
                    AddIdentityOrNot(column);
                }
            }
            return (keyFields, identityFields,nonIdentityFields);
        }

        #region INSERT METHODS





        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildInsertQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null)
        {
            var result = GetFields(dataTable);
            
            // Insert sql statement prefix 
            sqlBuilder.Append($"INSERT INTO {tableName ?? dataTable.TableName} (");

            // Add field names
            result.nonIdentityFields.ForEach(p => sqlBuilder.Append($"[{p}],"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Add parameter names for values
            sqlBuilder.Append(") VALUES (");
            result.nonIdentityFields.ForEach(p => sqlBuilder.Append($"@{p},"));
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
        private void BuildUpdateQuery(StringBuilder sqlBuilder,DataTable dataTable, string tableName = null) 
        {

            var result = GetFields(dataTable);

            if (result.keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessageForDataTable);
          

            // Build Update Statement Prefix
            sqlBuilder.Append($"UPDATE {tableName ?? dataTable.TableName} SET ");

            // Build Set fields
            result.nonIdentityFields.ForEach(p => sqlBuilder.Append($"[{p}]=@{p},"));
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

            // Build Where clause.
            sqlBuilder.Append(" ");
            BuildWhereClause(sqlBuilder, result.keyFields);
        }



        #endregion

        #region  DELETE METHODS




        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildDeleteQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null) 
        {
            var results = GetFields(dataTable);
            if (results.keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessageForDataTable);

            sqlBuilder.Append($"DELETE FROM {tableName ?? dataTable.TableName} ");
            BuildWhereClause(sqlBuilder, results.keyFields);
        }



        #endregion

        #region UPSERT METHODS


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null) 
        {
            var results = GetFields(dataTable);
            if (results.keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(sb, dataTable,tableName);
            var sb1 = new StringBuilder();
            BuildInsertQuery(sb1,dataTable, tableName);
            var sb2 = new StringBuilder();
            BuildWhereClause(sb2, results.keyFields);

            sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));

        }

        #endregion

        #region DB PARAMETERS


        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<T> BuildDbParameterList<T>(DataRow row, Func<string, object, T> GetNewParameter) where T : DbParameter
        {
            var list = new List<T>() { };
            foreach (DataColumn column in row.Table.Columns)
            {
                // if(column.AutoIncrement) continue;
                if (row.RowState == DataRowState.Deleted)
                {
                    list.Add(GetNewParameter($"@{column.ColumnName}", row[column.ColumnName,DataRowVersion.Original ]));
                }
                else
                {
                    list.Add(GetNewParameter($"@{column.ColumnName}", row[column.ColumnName]));
                }

            }
            return list;
        }

        #endregion

        /// <summary>
        /// Builds the where clause.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="keyFields">The key fields.</param>

        public void BuildWhereClause(StringBuilder sqlBuilder, List<string> keyFields)
        {
            if (keyFields.IsNullOrEmpty())
            {

            }
            else
            {
                sqlBuilder.Append("WHERE");
                keyFields.ForEach(p => sqlBuilder.Append($" [{p}]=@{p} AND"));
                if (sqlBuilder.ToString().EndsWith(" AND"))
                    sqlBuilder.Remove(sqlBuilder.Length - 4, 4); // Remove the last AND       
            }

        }



    }
}