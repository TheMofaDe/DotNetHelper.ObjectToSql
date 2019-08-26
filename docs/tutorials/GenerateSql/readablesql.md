## Creating Readable Sql

By default all sql is generated as **parameterized query** as they should be but there may be times where you
want to create non-parameterized queries. This article will show you how to do that


#### Create Readable Sql Using ObjectToSql Class

~~~csharp 
public class Employee {
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
~~~

~~~csharp                
var obj2Sql = new Services.ObjectToSql(DataBaseType.SqlServer);
// create an object you want to convert to sql
var employee = new Employee();

// create dbparameters from my object
var dbParameters = obj2Sql.BuildDbParameterList(employee, (s, o) => new SqlParameter(s, o));
// create my parameterized sql based on my specified action type
var insertSql = obj2Sql.BuildQuery<Employee>(ActionType.Insert);
// convert my parameterize sql to be readable
var readAbleSql = obj2Sql.SqlSyntaxHelper.ConvertParameterSqlToReadable(dbParameters, insertSql, Encoding.UTF8);
// unit test
Assert.AreEqual(readAbleSql, "INSERT INTO Employee ([FirstName],[LastName]) VALUES (NULL,NULL)");
~~~


#### Create Readable Sql Using DataTableToSql Class
            
~~~csharp
 // create an datatable you want to convert to sql
  var dt = new DataTable("Employee");
  dt.Columns.Add("IdentityKey", typeof(int));
  dt.Columns["IdentityKey"].AutoIncrement = true;
  dt.PrimaryKey = new[] { dt.Columns["IdentityKey"] };
  dt.Columns.Add("FirstName", typeof(string));
  dt.Columns.Add("LastName", typeof(string));
  dt.Rows.Add(1, "John", "Doe");
~~~
   
~~~csharp   
var dt2Sql = new Services.DataTableToSql(DataBaseType.SqlServer);

// create dbparameters from my datarow
var dbParameters = dt2Sql.BuildDbParameterList(dt.Rows[0], (s, o) => new SqlParameter(s, o));

// create my parameterized sql based on my specified action type
var insertSql = dt2Sql.BuildQuery(dt, ActionType.Insert);

// convert my parameterize sql to be readable
var readAbleSql = dt2Sql.SqlSyntaxHelper.ConvertParameterSqlToReadable(dbParameters, insertSql, Encoding.UTF8);
// unit test
Assert.AreEqual(readAbleSql, "INSERT INTO Employee ([FirstName],[LastName]) VALUES ('John','Doe')");
~~~  