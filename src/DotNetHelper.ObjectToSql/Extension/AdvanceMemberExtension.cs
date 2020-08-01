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


		//public static object GetMemberValue(this MemberWrapper member, object instanceOfObject , Func<object, string> xmlDeserializer, Func<object, string> jsonDeserializer, Func<object, string> csvDeserializer)
		//{
		//    var value = ExtFastMember.GetMemberValue(instanceOfObject, member.Name);
		//    var sqlAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
		//    if (sqlAttribute != null && sqlAttribute.SerializableType != SerializableType.NONE)
		//    {
		//        switch (sqlAttribute.SerializableType)
		//        {
		//            case SerializableType.XML:
		//                value = xmlDeserializer.Invoke(value);
		//                break;
		//            case SerializableType.JSON:
		//                value = jsonDeserializer.Invoke(value);
		//                break;
		//            case SerializableType.CSV:
		//                value = csvDeserializer.Invoke(value);
		//                break;
		//        }
		//    }
		//    return value;
		//}

		public static bool ShouldMemberBeIgnored(this MemberWrapper member)
		{
			var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
			if (attr1?.Ignore == true)
				return true;
			var attr2 = member.GetCustomAttribute<NotMappedAttribute>();
			if (attr2 != null)
				return true;
			return false;
		}

		public static bool IsMemberIgnoredForInsertSql(this MemberWrapper member)
		{
			var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
			if (attr1?.IsReadOnly == true)
				return true;
			return false;
		}

		public static bool IsMemberASerializableColumn(this MemberWrapper member)
		{
			if (member.ShouldMemberBeIgnored())
				return false;
			var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
			if (attr1?.SerializableType == null)
				return false;
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

		public static string GetNameFromCustomAttributeOrDefault(this MemberWrapper member)
		{
			var attr1 = member.GetCustomAttribute<SqlColumnAttribute>();
			var attr2 = member.GetCustomAttribute<ColumnAttribute>();
			if (!string.IsNullOrEmpty(attr1?.MapTo))
				return attr1.MapTo;
			if (!string.IsNullOrEmpty(attr2?.Name))
				return attr2.Name;
			return member.Name;
		}

		public static bool IsMemberAnIdentityColumn(this MemberWrapper member)
		{
			if (member.ShouldMemberBeIgnored())
				return false;
			var sqlColumnAttribute = member.GetCustomAttribute<SqlColumnAttribute>();
			var dataAnnotationAttribute = member.GetCustomAttribute<DatabaseGeneratedAttribute>();
			return
				 (dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity
				 || dataAnnotationAttribute?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
				 || (sqlColumnAttribute?.IsIdentityKey == true)
				;
		}

		public static bool IsMemberAPrimaryKeyColumn(this MemberWrapper member)
		{
			if (member.ShouldMemberBeIgnored())
				return false;
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