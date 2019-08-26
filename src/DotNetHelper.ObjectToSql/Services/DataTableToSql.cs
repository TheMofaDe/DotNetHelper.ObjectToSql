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
        public SqlSyntaxHelper SqlSyntaxHelper { get; }
        public DataTableToSql(DataBaseType type)
        {
            DatabaseType = type;
            SqlSyntaxHelper = new SqlSyntaxHelper(type);
        }

        #region Public Method Build Query

        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="actionType">INSERT,DELETE,UPDATE,OR UPSERT</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"> invalid actionType </exception>
        /// <exception cref="MissingKeyAttributeException"> can only be thrown for UPDATE,DELETE, & UPSERT Queries</exception> 
        public string BuildQuery(DataTable dataTable, ActionType actionType)
        {
            return BuildQuery(dataTable, actionType, dataTable.TableName);
        }


        /// <summary>
        /// Builds the query based on the specified actionType & table name
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="dataTable"></param>
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
                    BuildInsertQuery(sqlBuilder, dataTable, tableName);
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
        public string BuildQueryFromRowState(DataRow dataRow, string tableName)
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



        private (List<string> keyFields, List<string> identityFields, List<string> nonIdentityFields) GetFields(DataTable dataTable)
        {
            var identityFields = new List<string>() { };
            var keyFields = new List<string>() { };
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
            return (keyFields, identityFields, nonIdentityFields);
        }

        #region INSERT METHODS

        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="dataTable"></param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildInsertQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null)
        {
            var (_, _, nonIdentityFields) = GetFields(dataTable);
            sqlBuilder.Append(SqlGenerator.BuildInsertQuery(SqlSyntaxHelper, tableName ?? dataTable.TableName, nonIdentityFields, nonIdentityFields));

        }





        #endregion

        #region UPDATE METHODS

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="dataTable"></param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpdateQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null)
        {

            var (keyFields, identityFields, nonIdentityFields) = GetFields(dataTable);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessageForDataTable);

            sqlBuilder.Append($"{SqlGenerator.BuildUpdateTable(tableName ?? dataTable.TableName)} ");
            sqlBuilder.Append($"{SqlGenerator.BuildSetColumns(SqlSyntaxHelper, nonIdentityFields, nonIdentityFields)} ");
            sqlBuilder.Append($"{SqlGenerator.BuildWhereClause(SqlSyntaxHelper, keyFields, keyFields)}");


        }



        #endregion

        #region  DELETE METHODS

        /// <summary>
        /// Builds the delete query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="dataTable"></param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildDeleteQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null)
        {
            var (keyFields, identityFields, nonIdentityFields) = GetFields(dataTable);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessageForDataTable);

            sqlBuilder.Append(SqlGenerator.BuildDeleteQuery(SqlSyntaxHelper, tableName ?? dataTable.TableName, keyFields));
        }



        #endregion

        #region UPSERT METHODS


        private void SqLiteBuildUpsertQuery(StringBuilder sqlBuilder, List<string> keyFields, List<string> updateFields, string tableName, string whereClause, string normalInsertSQl, bool isAllKeyFieldsInt)
        {


            // var trueForAll = keyFields.TrueForAll(w => (w.Type == typeof(int) || w.Type == typeof(long))); // THESE ARE TREATED LKE IDENTITY FIELDS IF NOT SPECIFIED https://www.sqlite.org/autoinc.html
            if (isAllKeyFieldsInt)
            {
                sqlBuilder.Append($@"INSERT OR REPLACE INTO {tableName} 
({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))},{string.Join(",", updateFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))}) 
VALUES
({string.Join(",", keyFields.Select(w => $"(SELECT {w} FROM {tableName} {whereClause})"))}, {string.Join(",", updateFields.Select(w => $"@{w}"))} )");
            }
            else
            {
                sqlBuilder.Append($"{normalInsertSQl} ON CONFLICT ({string.Join(",", keyFields.Select(w => $"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{w}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}"))} DO UPDATE SET ");

                // Build Set fields
                updateFields.ForEach(p => sqlBuilder.Append($"{SqlSyntaxHelper.GetKeywordEscapeOpenChar()}{p}{SqlSyntaxHelper.GetKeywordEscapeClosedChar()}=@{p},"));
                sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma

                // Build Where clause.
                sqlBuilder.Append($" {whereClause}");
            }
        }


        /// <summary>
        /// Builds the upsert query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlBuilder">The SQL builder.</param>
        /// <param name="dataTable"></param>
        /// <param name="tableName">Name of the table.</param>
        private void BuildUpsertQuery(StringBuilder sqlBuilder, DataTable dataTable, string tableName = null)
        {
            var (keyFields, identityFields, nonIdentityFields) = GetFields(dataTable);
            if (keyFields.IsNullOrEmpty()) throw new MissingKeyAttributeException(ExceptionHelper.MissingKeyMessage);

            var sb = new StringBuilder();
            BuildUpdateQuery(sb, dataTable, tableName);
            var sb1 = new StringBuilder();
            BuildInsertQuery(sb1, dataTable, tableName);
            var sb2 = new StringBuilder();
            sb2.Append(SqlGenerator.BuildWhereClause(SqlSyntaxHelper, keyFields, keyFields));


            if (DatabaseType == DataBaseType.Sqlite)
            {
                var trueForAll = dataTable.PrimaryKey.AsList().TrueForAll(c => c.DataType == typeof(int) || c.DataType == typeof(long));

                SqLiteBuildUpsertQuery(sqlBuilder, keyFields, nonIdentityFields, tableName ?? dataTable.TableName, sb2.ToString(), sb1.ToString(), trueForAll);
            }
            else
            {
                sqlBuilder.Append(new SqlSyntaxHelper(DatabaseType).BuildIfExistStatement($"SELECT TOP 1 * FROM {tableName} {sb2}", sb.ToString(), sb1.ToString()));
            }




        }

        #endregion

        #region DB PARAMETERS


        /// <summary>
        /// Builds the SQL parameter list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List&lt;DbParameter&gt;.</returns>
        public List<T> BuildDbParameterList<T>(DataRow row, Func<string, object, T> getNewParameter) where T : DbParameter
        {
            var list = new List<T>() { };
            foreach (DataColumn column in row.Table.Columns)
            {
                // if(column.AutoIncrement) continue;
                list.Add(row.RowState == DataRowState.Deleted
                    ? getNewParameter($"@{column.ColumnName}", row[column.ColumnName, DataRowVersion.Original])
                    : getNewParameter($"@{column.ColumnName}", row[column.ColumnName]));
            }
            return list;
        }

        #endregion





    }
}