---
uid: TSD
---

## The 3 ways to generate sql

#### Objects to sql

```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}

var object2Sql = new ObjectToSql(DataBaseType.SqlServer);

// CREATE A INSERT,UPDATE,UPSERT,& DELETE SQL STATEMENT
var insertSQL = object2Sql.BuildQuery(ActionType.Insert,new Employee());
var updateSQL = object2Sql.BuildQuery(ActionType.Update,new Employee());
var upsertSQL = object2Sql.BuildQuery(ActionType.Upsert,new Employee());
var deleteSQL = object2Sql.BuildQuery(ActionType.Delete,new Employee());
```

#### DataTable to sql
```csharp
var dataTable2Sql = new DataTableToSql(DataBaseType.SqlServer);

// CREATE A INSERT,UPDATE,UPSERT,& DELETE SQL STATEMENT
var insertSQL = dataTable2Sql.BuildQuery(dataTable, ActionType.Insert);
var updateSQL = dataTable2Sql.BuildQuery(dataTable, ActionType.Update);
var upsertSQL = dataTable2Sql.BuildQuery(dataTable, ActionType.Upsert);
var deleteSQL = dataTable2Sql.BuildQuery(dataTable, ActionType.Delete);
```

#### Class to sql
```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}
var class2Sql = new ObjectToSql(DataBaseType.SqlServer);

// CREATE A INSERT,UPDATE,UPSERT,& DELETE SQL STATEMENT
var insertSQL = class2Sql.BuildQuery<Employee>(ActionType.Insert);
var updateSQL = class2Sql.BuildQuery<Employee>(ActionType.Update);
var upsertSQL = class2Sql.BuildQuery<Employee>(ActionType.Upsert);
var deleteSQL = class2Sql.BuildQuery<Employee>(ActionType.Delete);
```