using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetHelper.ObjectToSql.Tests.Models
{

    public class EmployeeWithIdentityKeyDataAnnotation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdentityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithManyPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [Key]
        public int PrimaryKey1 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithMappedColumnDataAnnotation
    {
        [Column(name: "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [Column(name: "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithIgnorePropertyDataAnnotation
    {
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeWithIgnorePropertyAndKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


}
