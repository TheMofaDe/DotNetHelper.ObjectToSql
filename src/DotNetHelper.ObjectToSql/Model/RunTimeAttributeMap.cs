using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DotNetHelper.FastMember.Extension;
using DotNetHelper.ObjectToSql.Attribute;
using DotNetHelper.ObjectToSql.Enum;

namespace DotNetHelper.ObjectToSql.Model
{
    public class RunTimeAttributeMap
    {
        public string PropertyName { get; }
        public List<System.Attribute> Attributes { get; }

        public RunTimeAttributeMap(string propertyName, List<System.Attribute> attributes)
        {
            PropertyName = propertyName;
            Attributes = attributes;
        }

        public T GetCustomAttribute<T>() where T : System.Attribute
        {
            return Attributes.FirstOrDefault(a => a is T) as T;
        }

    }

    public static class RunTimeAttributeMapExtension
    {

        public static object GetMemberValue(this RunTimeAttributeMap member, object instanceOfObject, Func<object, string> xmlDeserializer, Func<object, string> jsonDeserializer, Func<object, string> csvDeserializer)
        {
            var value = ExtFastMember.GetMemberValue(instanceOfObject, member.PropertyName);
            var sqlAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
            if (sqlAttribute != null && sqlAttribute.SerializableType != SerializableType.None)
            {
                switch (sqlAttribute.SerializableType)
                {
                    case SerializableType.Xml:
                        value = xmlDeserializer.Invoke(value);
                        break;
                    case SerializableType.Json:
                        value = jsonDeserializer.Invoke(value);
                        break;
                    case SerializableType.Csv:
                        value = csvDeserializer.Invoke(value);
                        break;
                }
            }
            return value;
        }

        public static bool ShouldMemberBeIgnored(this RunTimeAttributeMap member)
        {
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            if (attr1?.Ignore == true) return true;
            var attr2 = member.GetCustomAttribute<NotMappedAttribute>();
            if (attr2 != null) return true;
            return false;
        }

        public static bool IsMemberASerializableColumn(this RunTimeAttributeMap member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            if (attr1?.SerializableType == null) return false;
            switch (attr1.SerializableType)
            {
                case SerializableType.None:
                    return false;
                case SerializableType.Xml:
                    return true;
                case SerializableType.Json:
                    return true;
                case SerializableType.Csv:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public static string GetNameFromCustomAttributeOrDefault(this RunTimeAttributeMap member)
        {
            var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
            var attr2 = member.GetCustomAttribute<ColumnAttribute>();
            if (!string.IsNullOrEmpty(attr1?.MapTo)) return attr1.MapTo;
            if (!string.IsNullOrEmpty(attr2?.Name)) return attr2.Name;
            return member.PropertyName;
        }

        public static bool IsMemberAnIdentityColumn(this RunTimeAttributeMap member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var sqlColumnAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
            var dataAnnotationAttribute = member.GetCustomAttribute<DatabaseGeneratedAttribute>();
            return
                 (dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity
                 || dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
                 || (sqlColumnAttribute?.IsIdentityKey == true)
                ;
        }

        public static bool IsMemberAPrimaryKeyColumn(this RunTimeAttributeMap member)
        {
            if (member.ShouldMemberBeIgnored()) return false;
            var sqlColumnAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
            var dataAnnotationAttribute = member.GetCustomAttribute<DatabaseGeneratedAttribute>();
            var keyAttribute = member.GetCustomAttribute<KeyAttribute>();
            return
                (dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity
                 || dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
                 || (sqlColumnAttribute?.IsIdentityKey == true || sqlColumnAttribute?.PrimaryKey == true)
                 || (keyAttribute != null);
        }





    }

}

