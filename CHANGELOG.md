# Changelog
All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

<br/> 


## [2.0.0] - 2019-08-24

### Added
- MySQL support

### Changed
- Affect method *BuildDBParameters* . ConvertToDatabaseValue will no longer auto-convert value of DateTime.MinValue that belongs to a DateTime type property
  to sqlserver DateTime minimum supported value of *01,01,1753* see https://stackoverflow.com/a/12364243/2445462  
~~~csharp
 object ConvertToDatabaseValue(MemberWrapper member, object value, Func<object, string> XmlSerializer, Func<object, string> JsonSerializer, Func<object, string> CsvSerializer) 
~~~ 

### Removed 
- (**BREAKING CHANGE**) removed the following api from ObjectToSql 
~~~csharp
object ConvertToDatabaseValue(object value)
~~~ 

<br/> 

## [1.0.94] - 2019-08-22

### Changed
- The following overload now support anonymous object insert statement
~~~csharp
string BuildQuery<T>(ActionType actionType, string tableName = null) where T : class
~~~ 
<br/> 

## [1.0.93] - 2019-08-21

### Changed
-  removed primary keys from being set when creating update statements. This will allow support for anonymous object to  be inserted into a table have identity keys
<br/> 

## [1.0.91] - 2019-08-20

### Changed
-  Overload now supports update,upsert,delete action type for anonymous types
    ```csharp
    public string BuildQuery<T>(ActionType actionType, string tableName = null, params Expression<Func<T, object>>[] primaryKeys) where T : class
    ``` 

<br/> 

## [1.0.89] - 2019-08-17

### Changed
-  **BREAKING CHANGES**
    - Re-Order tableName parameter to the end of all public methods in ObjectToSql 
```csharp
// OLD API
public string BuildQuery(string tableName, ActionType actionType, object instance)
// NEW API
public string BuildQuery( ActionType actionType, object instance,string tableName)
```
<br/> 

## [1.0.62] - 2019-07-21

### Changed
-  Overload now supports insert action type for dynamic and anonymous types
    ```csharp
    public string BuildQuery(string tableName, ActionType actionType, object instance)
    ```
<br/> 

## [1.0.59] - 2019-07-20

### Changed
- fix bug where mapped column name was being used instead of property name in upsert sql only for sqlite targeting .
 
<br/> 

## [1.0.57] - 2019-07-20

### Changed
- Fix bug in upsert statement generatation for sqlite.



[1.0.57]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.57
[1.0.59]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.59
[1.0.62]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.62
[1.0.89]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.89
[1.0.91]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.91
[1.0.93]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.93
[1.0.94]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.94
[2.0.0]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v2.0.0



<!--  EXAMPLE SEE LINK https://keepachangelog.com/en/1.0.0/
## [0.0.1] - 2019-01-01
### Added
- add new way to be awesome.

### Changed
- changed how awesome is calculated.

### Removed
- removed the awe from awesome.
-->