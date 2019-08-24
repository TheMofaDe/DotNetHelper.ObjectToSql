using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests.Models
{

    public class EmployeeWithIdentityKeyDataAnnotation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdentityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithIdentityKeyDataAnnotation ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithIdentityKeyDataAnnotation SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [IdentityKey]=@IdentityKey";
                        case ActionType.Upsert:
                            return "IF EXISTS ( SELECT TOP 1 * FROM EmployeeWithIdentityKeySqlColumn WHERE [IdentityKey]=@IdentityKey ) BEGIN UPDATE EmployeeWithIdentityKeySqlColumn SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [IdentityKey]=@IdentityKey END ELSE BEGIN INSERT INTO EmployeeWithIdentityKeySqlColumn ([FirstName],[LastName]) VALUES (@FirstName,@LastName) END";
                        case ActionType.Delete:
                            return $"DELETE FROM EmployeeWithIdentityKeyDataAnnotation WHERE [IdentityKey]=@IdentityKey";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithIdentityKeyDataAnnotation (`FirstName`,`LastName`) VALUES (@FirstName,@LastName)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithIdentityKeyDataAnnotation SET `FirstName`=@FirstName,`LastName`=@LastName WHERE `IdentityKey`=@IdentityKey";
                        case ActionType.Upsert:
                            return "IF EXISTS ( SELECT TOP 1 * FROM EmployeeWithIdentityKeySqlColumn WHERE `IdentityKey`=@IdentityKey ) BEGIN UPDATE EmployeeWithIdentityKeySqlColumn SET `FirstName`=@FirstName,`LastName`=@LastName WHERE `IdentityKey`=@IdentityKey END ELSE BEGIN INSERT INTO EmployeeWithIdentityKeySqlColumn (`FirstName`,`LastName`) VALUES (@FirstName,@LastName) END";
                        case ActionType.Delete:
                            return $"DELETE FROM EmployeeWithIdentityKeyDataAnnotation WHERE `IdentityKey`=@IdentityKey";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Sqlite:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithIdentityKeyDataAnnotation ([FirstName],[LastName]) VALUES (@FirstName,@LastName)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithIdentityKeyDataAnnotation SET [FirstName]=@FirstName,[LastName]=@LastName WHERE [IdentityKey]=@IdentityKey";
                        case ActionType.Upsert:
                            return "INSERT OR REPLACE INTO EmployeeWithIdentityKeySqlColumn ([IdentityKey],[FirstName],[LastName]) VALUES ((SELECT IdentityKey FROM EmployeeWithIdentityKeySqlColumn WHERE [IdentityKey]=@IdentityKey), @FirstName,@LastName )";
                        //  return "INSERT OR REPLACE INTO EmployeeWithIdentityKeySqlColumn \r\n([IdentityKey],[FirstName],[LastName]) \r\nVALUES\r\n( (SELECT IdentityKey FROM EmployeeWithIdentityKeySqlColumn WHERE [IdentityKey]=@IdentityKey), @FirstName,@LastName )"; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return $"DELETE FROM EmployeeWithIdentityKeyDataAnnotation WHERE [IdentityKey]=@IdentityKey";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
        }
    }

    public class EmployeeWithPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.Sqlite:
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
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithPrimaryKeyDataAnnotation (`FirstName`,`LastName`,`PrimaryKey`) VALUES (@FirstName,@LastName,@PrimaryKey)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithPrimaryKeyDataAnnotation SET `FirstName`=@FirstName,`LastName`=@LastName WHERE `PrimaryKey`=@PrimaryKey";
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return $"DELETE FROM EmployeeWithPrimaryKeyDataAnnotation WHERE `PrimaryKey`=@PrimaryKey";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
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

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.Sqlite:
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
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithManyPrimaryKeyDataAnnotation (`FirstName`,`LastName`) VALUES (@FirstName,@LastName)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithManyPrimaryKeyDataAnnotation SET `FirstName`=@FirstName,`LastName`=@LastName WHERE `PrimaryKey`=@PrimaryKey AND `PrimaryKey1`=@PrimaryKey1";
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return $"DELETE FROM EmployeeWithManyPrimaryKeyDataAnnotation WHERE `PrimaryKey`=@PrimaryKey AND `PrimaryKey1`=@PrimaryKey1";
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
        }
    }

    public class EmployeeWithMappedColumnDataAnnotation
    {
        [Column("FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.Sqlite:
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
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithMappedColumnDataAnnotation (`FirstName2`,`LastName`) VALUES (@FirstName,@LastName)";
                        case ActionType.Update:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        default:

                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
        }
    }


    public class EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [Column("FirstName2")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
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

                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation (`FirstName2`,`LastName`,`PrimaryKey`) VALUES (@FirstName,@LastName,@PrimaryKey)";
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithMappedColumnAndPrimaryKeyDataAnnotation SET `FirstName2`=@FirstName,`LastName`=@LastName WHERE `PrimaryKey`=@PrimaryKey";
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }

                case DataBaseType.Sqlite:
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

                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
        }
    }



    public class EmployeeWithIgnorePropertyDataAnnotation
    {
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.Sqlite:
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
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO EmployeeWithIgnorePropertyDataAnnotation (`LastName`) VALUES (@LastName)";
                        case ActionType.Update:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }
            return null;
        }
    }

    public class EmployeeWithIgnorePropertyAndKeyDataAnnotation
    {
        [Key]
        public int PrimaryKey { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static string ToSql(ActionType action, DataBaseType dataBaseType)
        {
            switch (dataBaseType)
            {
                case DataBaseType.SqlServer:
                case DataBaseType.Sqlite:
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
                case DataBaseType.MySql:
                    switch (action)
                    {
                        case ActionType.Insert:
                            return $"INSERT INTO Employee (`FirstName`,`LastName`) VALUES (@FirstName,@LastName)"; // NO TEST CASE FOR tis
                        case ActionType.Update:
                            return $"UPDATE EmployeeWithIgnorePropertyAndKeyDataAnnotation SET `LastName`=@LastName WHERE `PrimaryKey`=@PrimaryKey";
                        case ActionType.Upsert:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        case ActionType.Delete:
                            return null; // SHOULD THROW EXCEPTIONS BECAUSE THERE IS NO KEYS
                        default:
                            throw new ArgumentOutOfRangeException(nameof(action), action, null);
                    }
                case DataBaseType.Oracle:
                    break;
                case DataBaseType.Oledb:
                    break;
                case DataBaseType.Access95:
                    break;
                case DataBaseType.Odbc:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataBaseType), dataBaseType, null);
            }

            return null;
        }
    }


}
