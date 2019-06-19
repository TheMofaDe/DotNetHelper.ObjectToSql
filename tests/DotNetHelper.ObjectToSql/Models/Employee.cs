using System;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Tests.Models
{
    public class Employee
    {
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
}
