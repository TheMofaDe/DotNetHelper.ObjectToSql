# DotNetHelper.ObjectToSql

#### *DotNetHelper.ObjectToSql takes your generic types or dynamic & anonymous objects and convert it to sql.* 

|| [**Documentation**][Docs] • [**API**][Docs-API] • [**Tutorials**][Docs-Tutorials] ||  [**Change Log**][Changelogs] • || [**View on Github**][Github]|| 

| AppVeyor | AzureDevOps |
| :-----: | :-----: |
| [![Build status](https://ci.appveyor.com/api/projects/status/0ogx4qcayyfnhkhk?svg=true)](https://ci.appveyor.com/project/TheMofaDe/dotnethelper-objecttosql)  | [![Build Status](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master)](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_build/latest?definitionId=5&branchName=master)  

| Package  | Tests | Code Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![codecov](https://codecov.io/gh/TheMofaDe/DotNetHelper.ObjectToSql/branch/master/graph/badge.svg)](https://codecov.io/gh/TheMofaDe/DotNetHelper.ObjectToSql) |


## Features
+ INSERT
+ UPDATE
+ DELETE
+ UPSERT
+ INSERT with OUTPUT Columns
+ UPDATE with OUTPUT Columns
+ DELETE with OUTPUT Columns
+ UPSERT with OUTPUT Columns

## Supported Databases
+ SQLSERVER
+ SQLITE
+ MYSQL
+ More to come

## How to use



##### How to Use With Generics Types

```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}
```
```csharp
var objectToSql = new ObjectToSql(DataBaseType.SqlServer);
var insertSql = objectToSql.BuildQuery<Employee>("TableName", ActionType.Insert);
// OR 
var insertSql = objectToSql.BuildQuery("TableName", ActionType.Insert,typeof(Employee));
```

##### How to Use With Dynamic Objects
```csharp
var objectToSql = new ObjectToSql(DataBaseType.SqlServer);
dynamic record = new ExpandoObject();
record.FirstName = "John";
record.LastName = "Doe";
var insertSql = objectToSql.BuildQuery("TableName", ActionType.Insert,record);
```


##### How to Use With Anonymous Objects
```csharp
var objectToSql = new ObjectToSql(DataBaseType.SqlServer);
var anonymousObject = new { FirstName = "John" , LastName = "Doe"}
var insertSql = objectToSql.BuildQuery("TableName", ActionType.Insert,anonymousObject);
```
##### Output
```sql
INSERT INTO TableNameGoHere ([FirstName],[LastName]) VALUES (@FirstName,@LastName)
```


## Documentation
For more information, please refer to the [Officials Docs][Docs] 

## Solution Template
[![badge](https://img.shields.io/badge/Built%20With-DotNet--Starter--Template-orange.svg)](https://github.com/TheMofaDe/DotNet-Starter-Template)


<!-- Links. -->

[1]:  https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[2]: http://themofade.github.io/DotNetHelper.ObjectToSql

[Cake]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Azure DevOps]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[AppVeyor]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[GitVersion]: https://gitversion.readthedocs.io/en/latest/
[Nuget]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Chocolately]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[WiX]: http://wixtoolset.org/
[DocFx]: https://dotnet.github.io/docfx/
[Github]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql


<!-- Documentation Links. -->
[Docs]: https://themofade.github.io/DotNetHelper.ObjectToSql/index.html
[Docs-API]: https://themofade.github.io/DotNetHelper.ObjectToSql/api/DotNetHelper.ObjectToSql.Attribute.html
[Docs-Tutorials]: https://themofade.github.io/DotNetHelper.ObjectToSql/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/


<!-- BADGES. -->

[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper.ObjectToSql.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/TheMofaDe/dotnethelper-objecttosql.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows


[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=macOS



