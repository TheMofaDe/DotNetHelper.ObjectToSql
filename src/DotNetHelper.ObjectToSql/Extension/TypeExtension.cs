using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using DotNetHelper.ObjectToSql.Attribute;

namespace DotNetHelper.ObjectToSql.Extension
{




    internal static class TypeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static (bool isNullableT, Type underlyingType) IsNullable(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var childType = Nullable.GetUnderlyingType(type);
                return (true, childType);
            }
            return (false, type);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeAnIEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsTypeDynamic(this Type type)
        {
            return typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type);
        }
        public static bool IsTypeAnonymousType(this Type type)
        {
            // https://stackoverflow.com/questions/2483023/how-to-test-if-a-type-is-anonymous
            return System.Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static Type GetEnumerableItemType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            Type elementType = null;

            if (type == typeof(IEnumerable))
            {
                elementType = typeof(object);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                elementType = type.GetGenericArguments()[0];
            }
            else
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType == typeof(IEnumerable))
                    {
                        elementType = typeof(object);
                    }
                    else if (interfaceType.IsGenericType &&
                             interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        elementType = interfaceType.GetGenericArguments()[0];
                        break;
                    }
                }
            }


            return elementType;
        }



        public static string GetNameFromCustomAttributeOrDefault(this Type type)
        {
            var t = type;
            while (type.IsTypeAnIEnumerable())
            {
                t = type.GetEnumerableItemType();
            }
            var sqlTableAttribute = t.GetCustomAttribute<SqlTableAttribute>(false);
            var tableAttribute = t.GetCustomAttribute<TableAttribute>(false);
            if (!string.IsNullOrEmpty(tableAttribute?.Name)) return tableAttribute.Name;
            if (!string.IsNullOrEmpty(sqlTableAttribute?.TableName)) return sqlTableAttribute.TableName;
            return t.Name;
        }


    }
}