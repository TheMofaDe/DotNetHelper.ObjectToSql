using System.Collections.Generic;

//using Configuration = CsvHelper.Configuration.Configuration;

namespace DotNetHelper.ObjectToSql.Extension
{
    internal static class ExtDictonary
    {
  




//   public static T DictionaryToObject<T>(this IOrderedDictionary dict)
//   {
//       var accessor = TypeAccessor.Create(typeof(T));
//       var t = accessor.CreateNew();
//       var props = accessor.GetMembers();
//       foreach (var key in dict.Keys)
//       {
//           if (props.Select(a => a.Name).ToList().Contains(key))
//           {
//               var p = props.First(b => string.Equals(b.Name, key.ToString(), StringComparison.CurrentCultureIgnoreCase));
//               var type = p.Type;
//               if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
//               {
//                   type = Nullable.GetUnderlyingType(type);
//               }
//               var value = dict.GetValue<object, object>(key);
//
//               if (type != null && value != null) value = Convert.ChangeType(value, type, null);
//               accessor[t, key.ToString()] = value;
//           }
//
//       }
//       return (T)t;
//   }


#if NETFRAMEWORK
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary.TryGetValue(key, out var a))
                    return a;
            }
            return defaultValue;
        }
#else
        public static V GetValueOrDefaultValue<K, V>(this IDictionary<K, V> dictionary, K key, V defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary.TryGetValue(key, out var a))
                    return a;
            }
            return defaultValue;
        }

#endif


        /// <summary>
        /// create key with value if not exist otherwise update value for key
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>return  new value </returns>
        public static V AddOrUpdate<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
            return value;
        }





    }
}
