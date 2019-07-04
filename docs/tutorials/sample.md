# Primary Key & Identity Fields
In the secnarios where you need to build Update,Delete, or Upsert Statements. Attributes are use to generate the where clause.

### Decorating Properties As Primary Key

```csharp
// USING SqlColumn Attribute
public class Employee {
      [SqlColumn(SetPrimaryKey = true)]
      public PrimaryKey {get; set;}
      public FirstName { get; set; }
      public LastName  { get; set; }
}
```
OR 
```csharp
// USING DataAnnotation Attribute
public class Employee {
      [Key]
      public PrimaryKey {get; set;}
      public FirstName { get; set; }
      public LastName  { get; set; }
}
```

using either version of the model above I can now generate update,delete, & upsert statment by doing the following

```csharp
   var actionType = ActionType.Update; // A enum with the values Insert,Update,Delete,Upsert
   var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
   var updateSql = sqlServerObjectToSql.BuildQuery<Employee>("Table Name Go Here If Null Defaults to Type Name",actionType);
   var upsertSql = sqlServerObjectToSql.BuildQuery<Employee>("Table Name Go Here If Null Defaults to Type Name",ActionType.Upsert);
   var deleteSql = sqlServerObjectToSql.BuildQuery<Employee>("Table Name Go Here If Null Defaults to Type Name",ActionType.Delete);

   Console.WriteLine(updateSql);
   Console.WriteLine(upsertSql);
   Console.WriteLine(deleteSql);
```
running the code above will produces the following sql statmente

```sql 
UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey
```
```sql 
UPDATE Employee SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey
```
```sql 
DELETE FROM EmployeeWithPrimaryKeySqlColumn WHERE [PrimaryKey]=@PrimaryKey
```

<!-- ### Supported Attributes 
this library has its own custom attributes and can also work with the common DataAnnotation attributes. With the support of DataAnnotation this means this library could be paired with your favorite orm like Dapper or Enitity Framework -->