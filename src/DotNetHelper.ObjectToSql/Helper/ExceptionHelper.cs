using System;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Helper
{
    internal static class ExceptionHelper
    {
        public static string MissingKeyMessage { get; } =
            "Can't build delete or Update statement without specifying the key properties." +
            $"{Environment.NewLine} You can use either or the following attributes  [SqlColumn(SetPrimaryKey = true)] OR [Key]" +
            $"{Environment.NewLine} For Identity properties use [DatabaseGenerated(DatabaseGeneratedOption.Identity)] OR [SqlColumn(SetIsIdentityKey = true)] )";

        public static string MissingIdentityKeyMessage(Type type)
        {

            return $"Can't build query with output because the type {type.FullName} isn't marked with any Identity Keys attribute. You can either execute an overload method that allows you to override which fields to " +
                   $"treat as identity keys or apply Identity field attribute [DatabaseGenerated(DatabaseGeneratedOption.Identity)] OR [SqlColumn(SetIsIdentityKey = true)]";
        }
        public static string NullSerializer(MemberWrapper member, SerializableType type)
        {

            return $"The property {member.Name} is marked with the Serializable attribute of type {type} but no implementation of a Serializer was provided";
        }

        public static string InvalidOperation_Overload_Doesnt_Support_ActionType_For_Type(ActionType actionType,string typeName)
        {
            return $"This overload doesn't support {typeName} type for action type {actionType}. " +
                   $"{Environment.NewLine} Please use the overload string BuildQuery<T>(string tableName, ActionType actionType, T instance, List<RunTimeAttributeMap> runTimeAttributes) where T : class " +
                   $"{Environment.NewLine}";
        }
    }
}
