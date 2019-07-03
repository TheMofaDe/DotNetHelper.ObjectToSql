# DotNetHelper.ObjectToSql

#### *DotNetHelper.ObjectToSql takes your generic types or dynamic & anonymous objects and convert it to sql.* 

|| [**Documentation**][Docs] • [**API**][Docs-API] • [**Getting Started**][Docs-getting-started] • [**Samples**](https://github.com/themofade/) || 

| AppVeyor | AzureDevOps |
| :-----: | :-----: |
| [![Build status](https://ci.appveyor.com/api/projects/status/0ogx4qcayyfnhkhk?svg=true)](https://ci.appveyor.com/project/TheMofaDe/dotnethelper-objecttosql)  | [![Build Status](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master)](https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_build/latest?definitionId=5&branchName=master)  

| Package  | Tests | Code Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![codecov](https://codecov.io/gh/TheMofaDe/DotNetHelper.ObjectToSql/branch/master/graph/badge.svg)](https://codecov.io/gh/TheMofaDe/DotNetHelper.ObjectToSql) |


#### Can create the following sql statements
+ INSERT
+ UPDATE
+ DELETE
+ UPSERT
+ INSERT with OUTPUT Columns
+ UPDATE with OUTPUT Columns
+ DELETE with OUTPUT Columns
+ UPSERT with OUTPUT Columns

#### Supports the following databases with more to come
+ SQLSERVER
+ SQLITE
+ MYSQL


## How to Use With Generics Types
```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var insertSql = sqlServerObjectToSql.BuildQuery<Employee>("TableNameGoesHere", ActionType.Insert);
// OR 
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert,typeof(Employee));
```

## How to Use With Dynamic Objects
```csharp
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert,record);
```


## How to Use With Anonymous Objects
```csharp
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var anonymousObject = new { FirstName = "John" , LastName = "Doe"}
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert,anonymousObject);
```
## Output
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


<!-- Documentation Links. -->
[Docs]: http://themofade.github.io/DotNetHelper.ObjectToSql
[Docs-API]: http://wixtoolset.org/
[Docs-getting-started]: https://dotnet.github.io/docfx/
[Docs-samples]: https://dotnet.github.io/docfx/


<!-- BADGES. -->

[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper.ObjectToSql.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/TheMofaDe/dotnethelper-objecttosql.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows


[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=macOS



