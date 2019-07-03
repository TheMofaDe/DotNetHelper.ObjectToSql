﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests.Models
{

    public class EmployeeWithIdentityKeyDataAnnotation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdentityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithIdentityKeyDataAnnotation ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return $"UPDATE EmployeeWithIdentityKeyDataAnnotation SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [IdentityKey]=@IdentityKey";
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return $"DELETE FROM EmployeeWithIdentityKeyDataAnnotation WHERE [IdentityKey]=@IdentityKey";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public class EmployeeWithPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithPrimaryKeyDataAnnotation ([FirstName],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey)";
                case ActionType.Update:
                    return $"UPDATE EmployeeWithPrimaryKeyDataAnnotation SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey";
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return $"DELETE FROM EmployeeWithPrimaryKeyDataAnnotation WHERE [PrimaryKey]=@PrimaryKey";
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
    public class EmployeeWithManyPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [Key]
        public int PrimaryKey1 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithManyPrimaryKeyDataAnnotation ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                case ActionType.Update:
                    return $"UPDATE EmployeeWithManyPrimaryKeyDataAnnotation SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey AND [PrimaryKey1]=@PrimaryKey1";
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return $"DELETE FROM EmployeeWithManyPrimaryKeyDataAnnotation WHERE [PrimaryKey]=@PrimaryKey AND [PrimaryKey1]=@PrimaryKey1"; 
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }

    public class EmployeeWithMappedColumnDataAnnotation
    {
        [Column("FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithMappedColumnDataAnnotation ([FirstName2],[LastName]) VALUES (@FirstName,@LastName)";
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


    public class EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [Column("FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation ([FirstName2],[LastName],[PrimaryKey]) VALUES (@FirstName,@LastName,@PrimaryKey)";
                case ActionType.Update:
                    return $"UPDATE EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation SET [FirstName2]=@FirstName,[LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey";
                case ActionType.Upsert:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                case ActionType.Delete:
                    return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }



    public class EmployeeWithIgnorePropertyDataAnnotation
    {
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action)
        {
            switch (action)
            {
                case ActionType.Insert:
                    return $"INSERT INTO EmployeeWithIgnorePropertyDataAnnotation ([LastName]) VALUES (@LastName)";
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

    public class EmployeeWithIgnorePropertyAndKeyDataAnnotation
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
                    return $"UPDATE EmployeeWithIgnorePropertyAndKeyDataAnnotation SET [LastName]=@LastName WHERE [PrimaryKey]=@PrimaryKey";
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
