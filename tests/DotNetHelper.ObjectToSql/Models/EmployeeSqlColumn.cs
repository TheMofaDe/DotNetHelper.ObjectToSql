using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetHelper.ObjectToSql.Attribute;

namespace DotNetHelper.ObjectToSql.Tests.Models
{
    public class EmployeeWithIdentityKeySqlColumn
    {
        [SqlColumn(SetIsIdentityKey = true)]
        public int IdentityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class EmployeeWithManyPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey1 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithMappedColumnSqlColumn
    {
        [SqlColumn(MapTo = "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class EmployeeWithMappedColumnAndPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        [SqlColumn(MapTo = "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }



    public class EmployeeWithIgnorePropertySqlColumn
    {
        [SqlColumn(SetIgnore = true)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithIgnorePropertyAndKeySqlColumn
    {
        [Key]
        public int PrimaryKey { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


}
