using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Extension;
using DotNetHelper.ObjectToSql.Helper;

namespace DotNetHelper.ObjectToSql.Model
{
	public class SqlTable
	{
		public DataBaseType DbType { get; private set; }
		public string TableName { get; private set; } = string.Empty;
		public string SchemaName { get; private set; } = string.Empty;
		public string DatabaseName { get; private set; } = string.Empty;

		public string FullNameWithBrackets => GetFullName(true);

		public string FullNameWithOutBrackets => GetFullName(false);


		public SqlTable(DataBaseType dbType, Type type)
		{
			DbType = dbType;
			var tableName = type.GetTableNameFromCustomAttributeOrDefault();
			Init(tableName);

		}


		public SqlTable(DataBaseType dbType, string tableName)
		{
			DbType = dbType;
			Init(tableName);

		}



		private void Init(string tableName)
		{
			if (tableName.Contains("."))
			{
				var splits = tableName.Split('.');
				if (splits.Length == 3) // database.schema.table
				{
					DatabaseName = RemoveBrackets(splits[0]);
					SchemaName = RemoveBrackets(splits[1]);
					TableName = RemoveBrackets(splits[2]);
				}
				else if (splits.Length == 2) // schema.table
				{
					SchemaName = RemoveBrackets(splits[0]);
					TableName = RemoveBrackets(splits[1]);
				}
				else if (splits.Length == 1) // .table
				{
					TableName = RemoveBrackets(splits[0]);
				}
			}
			else
			{
				TableName = RemoveBrackets(tableName);
			}
		}



		private string AddBrackets(string content)
		{
			var syntaxHelper = new SqlSyntaxHelper(DbType);
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
			var syntaxHelper = new SqlSyntaxHelper(DbType);
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