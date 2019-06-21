# DotNetHelper.ObjectToSql

##### DotNetHelper.ObjectToSql takes your generic types or dynamic & anonymous objects and convert it to sql. 

Supports Insert,Update,Delete,Upsert Statements



## How to Use With Generics Types
```csharp
public class Employee {
      public FirstName { get; set; }
      public LastName  { get; set; }
}
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var insertSql = sqlServerObjectToSql.BuildQuery<Employee>("TableNameGoesHere", ActionType.Insert);
// OR 
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert);
```

## How to Use With Dynamic Objects
```csharp
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            dynamic record = new ExpandoObject();
            record.FirstName = "John";
            record.LastName = "Doe";
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert,record,null);
```


## How to Use With Anonymous Objects
```csharp
            var sqlServerObjectToSql = new ObjectToSql(DataBaseType.SqlServer);
            var anonymousObject = new { FirstName = "John" , LastName = "Doe"}
            var insertSql = sqlServerObjectToSql.BuildQuery("TableNameGoesHere", ActionType.Insert,anonymousObject,null);
```
## Output
```sql
INSERT INTO TableNameGoHere ([FirstName],[LastName]) VALUES (@FirstName,@LastName)
```


## Targeted .NET Frameworks
    NET452
    NET45
    NETSTANDARD2.0


<h2>Fibonacci Generator</h2>
<p>Here is my example of a Fibonacci generator implemented in C#.</p>

<iframe src="https://try.dot.net/?fromGist=df44833326fcc575e8169fccb9d41fc7">
</iframe>