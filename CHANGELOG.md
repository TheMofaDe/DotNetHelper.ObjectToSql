# Changelog
All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),


## [1.0.91] - 2019-08-20

### Changed
-  Overload now supports update,upsert,delete action type for anonymous types
    ```csharp
    public string BuildQuery<T>(ActionType actionType, string tableName = null, params Expression<Func<T, object>>[] primaryKeys) where T : class
    ``` 

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

## [1.0.62] - 2019-07-21

### Changed
-  Overload now supports insert action type for dynamic and anonymous types
    ```csharp
    public string BuildQuery(string tableName, ActionType actionType, object instance)
    ```

## [1.0.59] - 2019-07-20

### Changed
- fix bug where mapped column name was being used instead of property name in upsert sql only for sqlite targeting .
 
## [1.0.57] - 2019-07-20

### Changed
- Fix bug in upsert statement generatation for sqlite.



[1.0.57]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.57
[1.0.59]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.59
[1.0.62]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.62
[1.0.89]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.89
[1.0.91]: https://github.com/TheMofaDe/DotNetHelper.ObjectToSql/releases/tag/v1.0.91




<!--  EXAMPLE SEE LINK https://keepachangelog.com/en/1.0.0/
## [0.0.1] - 2019-01-01
### Added
- add new way to be awesome.

### Changed
- changed how awesome is calculated.

### Removed
- removed the awe from awesome.
-->