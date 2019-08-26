---
uid: Tutorial.GenerateSQL.Datatable
---


## Primary Keys
```csharp
 var primaryKeys = new DataTable().PrimaryKey; 
 // THESE ARE YOUR PRIMARY KEYS COLUMNS
```
## Identity Fields
```csharp
DataColumn column;
if(column.AutoIncrement){
      // THEN THIS COLUMN WILL BE TREATED AS A IDENITTY FIELD 
}
```

## Creating SQL
 

```csharp
   var actionType = ActionType.Update; // A enum with the values Insert,Update,Delete,Upsert
   var dtToSql = new DataTableToSql(DataBaseType.SqlServer);
   var dt = new DataTable(); //
   var updateSql = dtToSql.BuildQuery(dt,actionType);
   var upsertSql = dtToSql.BuildQuery(dt,ActionType.Upsert);
   var deleteSql = dtToSql.BuildQuery(dt,ActionType.Delete);
```


> [!WARNING]
> Executing the a update,upsert, or delete query with a datatable that doesn't have any datacolumn declared as primary key will lead to an InvalidOperationException being thrown.  

## Creating DB Parameters From DataRow


```csharp
var parameters = dtToSql.BuildDbParameterList(new DataTable().Rows[0] (s, o) => new SqlParameter(s, o));
```


> [Tip]
> The method BuildDBParameterList has an overload that accepts Func<object, string> to allow for columns to be serialize for those senarios where your storing properties as json,csv or xml




<!-- ### Supported Attributes 
this library has its own custom attributes and can also work with the common DataAnnotation attributes. With the support of DataAnnotation this means this library could be paired with your favorite orm like Dapper or Enitity Framework -->

