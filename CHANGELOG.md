# Changelog
All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),

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



<!--  EXAMPLE SEE LINK https://keepachangelog.com/en/1.0.0/
## [0.0.1] - 2019-01-01
### Added
- add new way to be awesome.

### Changed
- changed how awesome is calculated.

### Removed
- removed the awe from awesome.
-->