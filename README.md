# DotNetHelper.ObjectToSql


| Package  | Tests | Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![Coverage Status](https://coveralls.io/repos/github/TheMofaDe/DotNetHelper.ObjectToSql/badge.svg)](https://coveralls.io/github/TheMofaDe/DotNetHelper.ObjectToSql) |

### *Azure DevOps*
| Windows | Linux | MacOS |
| :-----: | :-----: | :---: | 
| ![Build Status][azure-windows]  | ![Build Status][azure-linux]  | ![Build Status][azure-macOS] 

### *AppVeyor*
| Windows |
| :-----: | 
| ![Build Status][appveyor-windows]


##### DotNetHelper.ObjectToSql takes your generic types or dynamic & anonymous objects and convert it to sql. 

Supports Insert,Update,Delete,Upsert Statements



## How to Use With Generics Types
```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var insertSql = sqlServerObjectToSql.BuildQuery<Employee>("TableNameGoesHere", ActionType.Insert,null);
// OR 
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert, new Employee());
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
For more information, please refer to the [Officials Docs][2]

Created Using [DotNet-Starter-Template](http://themofade.github.io/DotNet-Starter-Template) 


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



<!-- BADGES. -->

[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper.ObjectToSql.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/themofade/DotNetHelper.ObjectToSql.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows

[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.ObjectToSql/_apis/build/status/TheMofaDe.DotNetHelper.ObjectToSql?branchName=master&jobName=macOS

[appveyor-windows]: https://ci.appveyor.com/project/TheMofaDe/DotNetHelper.ObjectToSql/branch/master
