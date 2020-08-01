using System;
using System.Linq.Expressions;
using DotNetHelper.ObjectToSql.Enum;
using DotNetHelper.ObjectToSql.Model;
using NUnit.Framework;

namespace DotNetHelper.ObjectToSql.Tests.SqlServerTest.Anonymous.Insert
{
	public class SqlServerAnonymousInsertFixture : BaseTest
	{



		[SetUp]
		public void Setup()
		{

		}
		[TearDown]
		public void Teardown()
		{

		}



		[Test]
		public void Test_Anonymous_T_BuildQuery_With_Specified_Table_Name()
		{

			string ReturnAsT<T>(T obj, DataBaseType type, ActionType actionType, params Expression<Func<T, object>>[] keyFields) where T : class
			{
				var objectToSql = new Services.ObjectToSql(type);
				return objectToSql.BuildQuery<T>(actionType, "Employee", keyFields);
			}

			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{

				var obj = new { FirstName = 1, LastName = "sfsd", Id = 2 };


				var insertSql = ReturnAsT(obj, type, ActionType.Insert, arg => arg.Id);
				var updateSql = ReturnAsT(obj, type, ActionType.Update, arg => arg.Id);
				var upsertSql = ReturnAsT(obj, type, ActionType.Upsert, arg => arg.Id);
				var deleteSql = ReturnAsT(obj, type, ActionType.Delete, arg => arg.Id);


				if (type == DataBaseType.MySql)
				{
					Assert.AreEqual(insertSql, "INSERT INTO Employee (`FirstName`,`Id`,`LastName`) VALUES (@FirstName,@Id,@LastName)");
					Assert.AreEqual(updateSql, "UPDATE Employee SET `FirstName`=@FirstName,`LastName`=@LastName WHERE `Id`=@Id");
				}
				else
				{
					Assert.AreEqual(insertSql, "INSERT INTO Employee ([FirstName],[Id],[LastName]) VALUES (@FirstName,@Id,@LastName)");
					Assert.AreEqual(updateSql, "UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [Id]=@Id");
				}
				if (type == DataBaseType.SqlServer)
					Assert.AreEqual(upsertSql, "IF EXISTS ( SELECT TOP 1 * FROM Employee WHERE [Id]=@Id ) BEGIN UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [Id]=@Id END ELSE BEGIN INSERT INTO Employee ([FirstName],[Id],[LastName]) VALUES (@FirstName,@Id,@LastName) END");
				if (type == DataBaseType.Sqlite)
					Assert.AreEqual(upsertSql, "INSERT OR REPLACE INTO Employee ([Id],[FirstName],[Id],[LastName]) VALUES ( (SELECT Id FROM Employee WHERE [Id]=@Id), @FirstName,@Id,@LastName )");

				if (type == DataBaseType.MySql)
				{
					Assert.AreEqual(deleteSql, "DELETE FROM Employee WHERE `Id`=@Id");
				}
				else
				{
					Assert.AreEqual(deleteSql, "DELETE FROM Employee WHERE [Id]=@Id");
				}
			});
		}

		[Test]
		public void Test_Anonymous_T_BuildQuery_INSERT_Doesnt_Throw_Exception_When_Key_Are_Not_Specified()
		{
			string ReturnAsT<T>(T obj, DataBaseType type, ActionType actionType, params Expression<Func<T, object>>[] keyFields) where T : class
			{
				var objectToSql = new Services.ObjectToSql(type);
				return objectToSql.BuildQuery<T>(actionType, "Employee", keyFields);
			}

			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var obj = new { FirstName = 1, LastName = "sfsd", Id = 2 };
				var insertSql = ReturnAsT(obj, type, ActionType.Insert);
			});
		}


		[Test]
		public void Test_Anonymous_BuildInsertQuery_With_Specified_Table_Name()
		{
			RunTestOnAllDBTypes(delegate (DataBaseType type)
			{
				var obj = new { FirstName = 1, LastName = "sfsd" };
				var objectToSql = new Services.ObjectToSql(type);
				var sql = objectToSql.BuildQuery(ActionType.Insert, obj, "Table");
				if (type == DataBaseType.MySql)
				{
					Assert.AreEqual(sql, "INSERT INTO Table (`FirstName`,`LastName`) VALUES (@FirstName,@LastName)");
				}
				else
				{
					Assert.AreEqual(sql, "INSERT INTO Table ([FirstName],[LastName]) VALUES (@FirstName,@LastName)");
				}
			});
		}






	}
}