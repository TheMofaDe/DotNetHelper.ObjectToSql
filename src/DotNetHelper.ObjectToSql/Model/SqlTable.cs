using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql.Model
{
    public class SQLTable
    {
        public DataBaseType DBType { get; private set; }
        public string TableName { get; private set; } = string.Empty;
        public string SchemaName { get; private set; } = string.Empty;
        public string DatabaseName { get; private set; } = string.Empty;

        public string FullNameWithBrackets => GetFullName(true);

        public string FullNameWithOutBrackets => GetFullName(false);


        public SQLTable(DataBaseType dbType, Type type)
        {
            DBType = dbType;
            var tableName = type.GetNameFromCustomAttributeOrDefault();
            Init(tableName);

        }


        public SQLTable(DataBaseType dbType, string tableName)
        {
            DBType = dbType;
            Init(tableName);

        }



        private void Init(string tableName)
        {
            if (tableName.Contains("."))
            {
                var splits = tableName.Split('.');
                if (splits.Length == 3) // database.schema.table
                {
                    DatabaseName = splits[0];
                    SchemaName = splits[1];
                    TableName = splits[2];
                }
                else if (splits.Length == 2) // schema.table
                {
                    SchemaName = splits[0];
                    TableName = splits[1];
                }
                else if (splits.Length == 1) // .table
                {
                    TableName = splits[0];
                }
            }
            else
            {
                TableName = tableName;
            }
        }



        private string AddBrackets(string content)
        {
            var syntaxHelper = new SqlSyntaxHelper(DBType);
            if (!content.StartsWith(syntaxHelper.GetKeywordEscapeOpenChar()))
            {
                content = $"{syntaxHelper.GetKeywordEscapeOpenChar()}{content}";
            }
            if (!content.EndsWith(syntaxHelper.GetKeywordEscapeClosedChar()))
            {
                content = $"{content}{syntaxHelper.GetKeywordEscapeClosedChar()}";
            }
            return content;
        }

        private string RemoveBrackets(string content)
        {
            var syntaxHelper = new SqlSyntaxHelper(DBType);
            if (content.StartsWith(syntaxHelper.GetKeywordEscapeOpenChar()))
            {
                content = content.ReplaceFirstOccurrence(syntaxHelper.GetKeywordEscapeOpenChar(), string.Empty, StringComparison.Ordinal);
            }
            if (content.EndsWith(syntaxHelper.GetKeywordEscapeClosedChar()))
            {
                content = content.ReplaceLastOccurrence(syntaxHelper.GetKeywordEscapeClosedChar(), string.Empty, StringComparison.Ordinal);
            }
            return content;
        }

        private string GetFullName(bool includeBrackets)
        {

            if (!string.IsNullOrEmpty(DatabaseName))
            {
                return includeBrackets
                    ? $"{AddBrackets(DatabaseName)}.{AddBrackets(SchemaName)}.{AddBrackets(TableName)}"
                    : $"{RemoveBrackets(DatabaseName)}.{RemoveBrackets(SchemaName)}.{RemoveBrackets(TableName)}";
            }

            if (!string.IsNullOrEmpty(SchemaName))
            {
                return includeBrackets
                    ? $"{AddBrackets(SchemaName)}.{AddBrackets(TableName)}"
                    : $"{RemoveBrackets(SchemaName)}.{RemoveBrackets(TableName)}";
            }
            if (!string.IsNullOrEmpty(TableName))
            {
                return includeBrackets
                    ? $"{AddBrackets(TableName)}"
                    : $"{RemoveBrackets(TableName)}";
            }

            return string.Empty;
        }

    }
}
