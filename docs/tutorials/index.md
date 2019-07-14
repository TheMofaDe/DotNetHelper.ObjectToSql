---
uid: TSD
---

## The 2 ways to generate sql

#### Objects to sql

```csharp
var object2Sql = new ObjectToSql(DataBaseType.SqlServer);
```

#### DataTable to sql
```csharp
var object2Sql = new DataTableToSql(DataBaseType.SqlServer);
```