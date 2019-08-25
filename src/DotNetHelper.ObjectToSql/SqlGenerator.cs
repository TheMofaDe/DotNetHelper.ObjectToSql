using System;
using System.Collections.Generic;
using System.Text;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql
{
   public static class SqlGenerator
   {

       public static string BuildValues(SqlSyntaxHelper syntax, List<string> columns)
       {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"{syntax.Const_Values} {syntax.Const_Open_Parens}"); // VALUES (
            columns.ForEach(s => sqlBuilder.Append($"{syntax.Const_At}{s},"));  // @test,
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
            sqlBuilder.Append(syntax.Const_Close_Parens); // ) 
            return sqlBuilder.ToString(); // VALUES (@TEST,TEST)
       }

       public static string BuildColumnsInParentheses(SqlSyntaxHelper syntax, List<string> columns)
       {
           var sqlBuilder = new StringBuilder();
           var o = syntax.GetKeywordEscapeOpenChar(); // alias to keep code short    [
           var c = syntax.GetKeywordEscapeClosedChar(); // alias to keep code short  ]
            sqlBuilder.Append($"{syntax.Const_Open_Parens}"); // (
           columns.ForEach(s => sqlBuilder.Append($"{o}{s}{c},"));  // [test],[test2]
           sqlBuilder.Remove(sqlBuilder.Length - 1, 1); // Remove the last comma
           sqlBuilder.Append($"{syntax.Const_Close_Parens}"); // )
           return sqlBuilder.ToString(); // ([test],[test2])
        }


        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public static string BuildInsertQuery(SqlSyntaxHelper syntax, string tableName, List<string> columns, List<string> valueColumns)
        {

            var columnsInParenthesesSection = BuildColumnsInParentheses(syntax, columns);
            var valueSection = BuildValues(syntax, valueColumns);
            return $"{syntax.Const_Insert_Into} {tableName} {columnsInParenthesesSection} {valueSection}";
            // INSERT INTO TABLE (A,B,C) VALUES (@A,@B,@C)
        }


    }
}
