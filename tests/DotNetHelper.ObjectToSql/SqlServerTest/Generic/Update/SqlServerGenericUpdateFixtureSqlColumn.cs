using System;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Tests.Models;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Generic.Update
{
	public class SqlServerGenericUpdateFixtureSqlColumn : BaseTest
	{

		public ActionType ActionType { get; } = ActionType.Update;

		[SetUp]
		public void Setup()
		{

		}
		[TearDown]
		public void Teardown()
		{

		}

		[Test]
		public void Test_Generic_BuildUpdateQuery_Uses_MappedColumn_Name_Instead_Of_PropertyName()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithMappedColumnAndPrimaryKeySqlColumn>(ActionType, nameof(EmployeeWithMappedColumnAndPrimaryKeySqlColumn));
				Assert.AreEqual(sql, EmployeeWithMappedColumnAndPrimaryKeySqlColumn.ToSql(ActionType, type));
			});
		}

		[Test]
		public void Test_Generic_BuildUpdateQuery_Doesnt_Include_Ignored_Column()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithIgnorePropertyAndKeySqlColumn>(ActionType, nameof(EmployeeWithIgnorePropertyAndKeySqlColumn));
				Assert.AreEqual(sql, EmployeeWithIgnorePropertyAndKeySqlColumn.ToSql(ActionType, type));
			});
		}

		[Test]
		public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Identity_Column()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType, nameof(EmployeeWithIdentityKeySqlColumn));
				Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType, type));
			});
		}

		[Test]
		public void Test_Generic_Ensure_Table_Attribute_Name_Is_Used()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithTableAttribute>(ActionType);
				Assert.AreEqual(sql, EmployeeWithTableAttribute.ToSql(ActionType, type));
			});
		}

		[Test]
		public void Test_Generic_BuildQuery_Ensure_Override_Keys_Is_Used()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithIdentityKeySqlColumn>(ActionType, nameof(EmployeeWithIdentityKeySqlColumn), column => column.IdentityKey);
				Assert.AreEqual(sql, EmployeeWithIdentityKeySqlColumn.ToSql(ActionType, type));
			});
		}

		[Test]
		public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Primary_Column()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>(ActionType, nameof(EmployeeWithPrimaryKeySqlColumn));
				Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType, type));
			});

		}

		[Test]
		public void Test_Generic_BuildUpdateQuery_Includes_Where_Clause_With_Multiple_Primary_Column()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery<EmployeeWithManyPrimaryKeySqlColumn>(ActionType, nameof(EmployeeWithManyPrimaryKeySqlColumn));
				Assert.AreEqual(sql, EmployeeWithManyPrimaryKeySqlColumn.ToSql(ActionType, type));
			});
		}


		[Test]
		public void Test_Generic_BuildQueryWithOutputs()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var objectToSql = new Services.ObjectToSql(type);

				var sql = string.Empty;
				if (type == DataBaseType.Sqlite || type == DataBaseType.MySql)
				{
					EnsureExpectedExceptionIsThrown<NotImplementedException>(() =>
						objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(ActionType,
							"Employee", a => a.PrimaryKey)
					);
					return;
				}
				else
				{

					sql = objectToSql.BuildQueryWithOutputs<EmployeeWithPrimaryKeySqlColumn>(
					   ActionType, nameof(Employee), a => a.PrimaryKey);
				}

				Assert.AreEqual(sql, $@"UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName OUTPUT DELETED.[PrimaryKey] WHERE [PrimaryKey]=@PrimaryKey");
			});
		}

		//[Test]
		//public void Test_Generic_BuildUpdateQuery_Ignores_All_Keys_Attributes_And_Uses_Only_OverrideKeys()
		//{
		//    var objectToSql = new Services.ObjectToSql(type);
		//    var sql = objectToSql.BuildQuery<EmployeeWithPrimaryKeySqlColumn>( ActionType, null);
		//    Assert.AreEqual(sql, EmployeeWithPrimaryKeySqlColumn.ToSql(ActionType,type));
		//}




	}
}