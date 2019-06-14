using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests.Models
{
    public class EmployeeWithIdentityKeySqlColumn
    {
        [SqlColumn(SetIsIdentityKey = true)]
        public int IdentityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithIdentityKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public class EmployeeWithPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithPrimaryKeySqlColumn ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
    public class EmployeeWithManyPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey1 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithManyPrimaryKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public class EmployeeWithMappedColumnSqlColumn
    {
        [SqlColumn(MapTo = "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithMappedColumnSqlColumn ([FirstName2],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }


    public class EmployeeWithMappedColumnAndPrimaryKeySqlColumn
    {
        [SqlColumn(SetPrimaryKey = true)]
        public int PrimaryKey { get; set; }
        [SqlColumn(MapTo = "FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }



    public class EmployeeWithIgnorePropertySqlColumn
    {
        [SqlColumn(SetIgnore = true)]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithIgnorePropertySqlColumn ([LastName]) VALUES (@LastName)";
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public class EmployeeWithIgnorePropertyAndKeySqlColumn
    {
        [Key]
        public int PrimaryKey { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO Employee ([FirstName],[LastName]) VALUES (@FirstName,@LastName)"; // NO TEST CASE FOR tis
                case ActionType.Update:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }


}
