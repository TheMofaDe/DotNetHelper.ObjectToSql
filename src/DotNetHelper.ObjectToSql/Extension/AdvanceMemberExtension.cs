using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetHelper.FastMember.Extension.Models;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Extension
{
   public static class MemberWrapperExtension
   {
        //internal static SqlColumnAttribute GetSqlColumnAttribute(this MemberWrapper member)
        //{
        //    return member.GetCustomAttribute<SqlColumnAttribute>();
        //}

        //internal static ColumnAttribute GetColumnAttribute(this MemberWrapper member)
        //{
        //    return member.GetCustomAttribute<ColumnAttribute>();
        //}

        //internal static DatabaseGeneratedAttribute GetDatabaseGeneratedAttribute(this MemberWrapper member)
        //{
        //    return member.GetCustomAttribute<DatabaseGeneratedAttribute>();
        //}




        public static bool ShouldMemberBeIgnored(this MemberWrapper member)
        {
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            if (attr1?.Ignore == true) return true;
            var attr2 = member.GetCustomAttribute<NotMappedAttribute>();
            if (attr2 != null) return true;
            return false;
        }

        public static bool IsMemberASerializableColumn(this MemberWrapper member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            if(attr1?.SerializableType == null) return false;
            switch (attr1.SerializableType)
            {
                case SerializableType.NONE:
                    return false;
                case SerializableType.XML:
                    return true;
                case SerializableType.JSON:
                    return true;
                case SerializableType.CSV:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
        }

        public static string GetNameFromCustomAttributeOrDefault(this MemberWrapper member)
        {
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            var attr2 = member.GetCustomAttribute<ColumnAttribute>();
            if (!string.IsNullOrEmpty(attr1?.MapTo)) return attr1.MapTo;
            if (!string.IsNullOrEmpty(attr2?.Name)) return attr2.Name;
            return member.Name;
        }

        public static bool IsMemberAnIdentityColumn(this MemberWrapper member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var sqlColumnAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
            var dataAnnonationAttribute = member.GetCustomAttribute<DatabaseGeneratedAttribute>();
            return
                 (  dataAnnonationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity
                 || dataAnnonationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
                 || (sqlColumnAttribute?.IsIdentityKey == true) 
                ;
        }

        public static bool IsMemberAPrimaryKeyColumn(this MemberWrapper member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var sqlColumnAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
            var dataAnnonationAttribute = member.GetCustomAttribute<DatabaseGeneratedAttribute>();
            var keyAttribute = member.GetCustomAttribute<KeyAttribute>();
            return
                (dataAnnonationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity
                 || dataAnnonationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
                 || (sqlColumnAttribute?.IsIdentityKey == true || sqlColumnAttribute?.PrimaryKey == true)
                 || (keyAttribute != null);
        }
    }
}
