using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql
{
    public static class SqlGenerator
    {
        /// <summary>
        /// Example *UPDATE TABLE*
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string BuildUpdateTable(string tableName)
        {
            return $"UPDATE {tableName}";
        }

        /// <summary>
        /// Example *DELETE FROM TABLE*
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string BuildDeleteFromTable(string tableName)
        {
            return $"DELETE FROM {tableName}";
        }

        /// <summary>
        /// Example *INSERT OR REPLACE INTO TABLE*
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string BuildInsertOrReplace(string tableName)
        {
            return $"INSERT OR REPLACE INTO {tableName}";
        }

        /// <summary>
        /// Example *INSERT INTO TABLE*
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static string BuildInsertIntoTable(SqlSyntaxHelper syntax, string tableName)
        {
            return $"{syntax.ConstInsertInto} {tableName}";
        }
        /// <summary>
        /// Example *VALUES (@A,@B,@C)*
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        internal static string BuildValues(SqlSyntaxHelper syntax, List<string> columns)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"{syntax.ConstValues} {syntax.ConstOpenParens}"); // VALUES (
            columns.ForEach(s => sqlBuilder.Append($"{syntax.ConstAt}{s},"));  // @test,
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(syntax.ConstCloseParens); // ) 
            return sqlBuilder.ToString(); // VALUES (@TEST,TEST)
        }

        /// <summary>
        /// Example *([A],[B],[C])*
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        internal static string BuildColumnsInParentheses(SqlSyntaxHelper syntax, List<string> columns)
        {
            var sqlBuilder = new StringBuilder();
            var o = syntax.GetKeywordEscapeOpenChar(); // alias to keep code short    [
            var c = syntax.GetKeywordEscapeClosedChar(); // alias to keep code short  ]
            sqlBuilder.Append($"{syntax.ConstOpenParens}"); // (
            columns.ForEach(s => sqlBuilder.Append($"{o}{s}{c},"));  // [test],[test2]
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append($"{syntax.ConstCloseParens}"); // )
            return sqlBuilder.ToString(); // ([test],[test2])
        }

        /// <summary>
        /// Example *SET A=@A, B=@b, C=@C*
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="columns"></param>
        /// <param name="parameterColumns"></param>
        /// <returns></returns>
        internal static string BuildSetColumns(SqlSyntaxHelper syntax, List<string> columns, List<string> parameterColumns)
        {
            var sqlBuilder = new StringBuilder("SET ");
            var i = 0;
            foreach (var col in columns)
            {
                sqlBuilder.Append($"{syntax.GetKeywordEscapeOpenChar()}{col}{syntax.GetKeywordEscapeClosedChar()}=@{parameterColumns[i]},");
                i++;
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            return sqlBuilder.ToString();
        }


        /// <summary>
        /// Example *OUTPUT INSERTED.A , INSERTED.B, INSERTED.C*
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="syntax"></param>
        /// <param name="outputFields"></param>
        /// <returns></returns>
        internal static string BuildOutputFields<T>(SqlSyntaxHelper syntax, List<string> outputFields, OutputType outputType) where T : class
        {
            var o = syntax.GetKeywordEscapeOpenChar(); // alias to keep code short    [
            var c = syntax.GetKeywordEscapeClosedChar(); // alias to keep code short  ]
            var sqlBuilder = new StringBuilder();
            //sqlBuilder.Append($"{Environment.NewLine} OUTPUT");
            sqlBuilder.Append($"OUTPUT");
            var members = ExtFastMember.GetMemberWrappers<T>(true);
            outputFields.ForEach(delegate (string s)
            {
                sqlBuilder.Append($" {outputType}.{o}{members.First(av => av.Name == s).GetNameFromCustomAttributeOrDefault()}{c} ,");
            });
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            return sqlBuilder.ToString();
        }



        /// <summary>
        /// BUilds a where clause from a list of member wrappers
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="members"> </param>
        /// <returns></returns>
        public static string BuildWhereClauseFromMembers(SqlSyntaxHelper syntax, List<MemberWrapper> members)
        {
            // This uses the .net property attribute mapto else default property name
            var columns = members.Select(c => c.GetNameFromCustomAttributeOrDefault()).AsList();
            // This uses the .net property name & ignores any attribute mapto name to ensure duplication is prevented
            var values = members.Select(c => c.Name).AsList();
            return BuildWhereClause(syntax, columns, values);
        }

        /// <summary>
        /// Builds the where clause.
        /// </summary>
        public static string BuildWhereClause(SqlSyntaxHelper syntax, List<string> columns, List<string> parameterColumns)
        {
            if (columns.IsNullOrEmpty())
            {
                return string.Empty;
            }
            else
            {
                var sqlBuilder = new StringBuilder("WHERE");
                var i = 0;
                foreach (var col in columns)
                {
                    sqlBuilder.Append($" {syntax.GetKeywordEscapeOpenChar()}{col}{syntax.GetKeywordEscapeClosedChar()}=@{parameterColumns[i]} AND");
                    i++;
                }
                sqlBuilder.Remove(sqlBuilder.Length - 4, 4); // Remove the last AND       
                return sqlBuilder.ToString();
            }

        }



        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public static string BuildInsertQuery(SqlSyntaxHelper syntax, string tableName, List<string> columns, List<string> valueColumns)
        {

            var columnsInParenthesesSection = BuildColumnsInParentheses(syntax, columns);
            var valueSection = BuildValues(syntax, valueColumns);
            return $"{BuildInsertIntoTable(syntax, tableName)} {columnsInParenthesesSection} {valueSection}";
            // INSERT INTO TABLE (A,B,C) VALUES (@A,@B,@C)
        }

        /// <summary>
        /// Builds an update query
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="valueColumns"></param>
        /// <returns></returns>
        //public static string BuildUpdateQuery(SqlSyntaxHelper syntax, string tableName, List<string> columns, List<string> valueColumns)
        //{
        //    var updateTableClause = ($"{SqlGenerator.BuildUpdateTable(tableName)} ");
        //    var setColumnsClause = ($"{SqlGenerator.BuildSetColumns(syntax, columns, valueColumns)} ");
        //    var whereClause = BuildWhereClause(syntax, columns, valueColumns);
        //    return $"{updateTableClause}{setColumnsClause}{whereClause}";
        //}

        public static string BuildDeleteQuery(SqlSyntaxHelper syntax, string tableName, List<string> keyColumns)
        {
            var deleteFromClause = ($"{SqlGenerator.BuildDeleteFromTable(tableName)} ");
            var whereClause = ($"{SqlGenerator.BuildWhereClause(syntax, keyColumns, keyColumns)}");
            return $"{deleteFromClause}{whereClause}";
        }
        public static string BuildDeleteQuery(SqlSyntaxHelper syntax, string tableName, List<MemberWrapper> keyColumns)
        {
            var deleteFromClause = ($"{SqlGenerator.BuildDeleteFromTable(tableName)} ");
            var whereClause = ($"{SqlGenerator.BuildWhereClauseFromMembers(syntax, keyColumns)}");
            return $"{deleteFromClause}{whereClause}";
        }


    }
}
