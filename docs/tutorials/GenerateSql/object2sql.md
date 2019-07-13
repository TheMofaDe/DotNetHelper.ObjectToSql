---
uid: Tutorial.GenerateSQL
---

# Primary Key & Identity Fields
In the secnarios where you need to build Update,Delete, or Upsert Statements. Attributes are use to generate the where clause.

### Decorating Properties As Primary Key

```csharp
// USING SqlColumn Attribute
public class Employee {
      [SqlColumn(SetPrimaryKey = true)]
      public int PrimaryKey {get; set;}
      public string FirstName { get; set; }
      public string LastName  { get; set; }
}
```
OR 
```csharp
// USING DataAnnotation Attribute
public class Employee {
      [Key]
      public int PrimaryKey {get; set;}
      public string FirstName { get; set; }
      public string LastName  { get; set; }
}
```

using either version of the model above I can now generate update,delete, & upsert statment by doing the following
 

```csharp
   var actionType = ActionType.Update; // A enum with the values Insert,Update,Delete,Upsert
   var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
   var updateSql = sqlServerObjectToSql.BuildQuery<Employee>(null,actionType);
   var upsertSql = sqlServerObjectToSql.BuildQuery<Employee>("Employee",ActionType.Upsert);
   var deleteSql = sqlServerObjectToSql.BuildQuery<Employee>("TableName",ActionType.Delete);

   Console.WriteLine(updateSql);
   Console.WriteLine(upsertSql);
   Console.WriteLine(deleteSql);
```
running the code above will produces the following sql statmente

```sql 
UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey
```
```sql 
IF EXISTS ( SELECT TOP 1 * FROM Employee WHERE [PrimaryKey]=@PrimaryKey ) BEGIN UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey END ELSE BEGIN INSERT INTO Employee ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey) END
```
```sql 
DELETE FROM TableName WHERE [PrimaryKey]=@PrimaryKey
```

> [!WARNING]
> Executing the a update,upsert, or delete query with an object that doesn't have any key attributes will lead to an InvalidOperationException being thrown.  

## Creating DB Parameters From Object

```csharp
var parameters = sqlServerObjectToSql.BuildDbParameterList(new Employee(), (s, o) => new SqlParameter(s, o),null,null,null);
```



<!-- ### Supported Attributes 
this library has its own custom attributes and can also work with the common DataAnnotation attributes. With the support of DataAnnotation this means this library could be paired with your favorite orm like Dapper or Enitity Framework -->

