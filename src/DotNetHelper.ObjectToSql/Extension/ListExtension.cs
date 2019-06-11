using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNetHelper.ObjectToSql.Extension
{
    internal static class ListExtension
    {


        /// <summary>
        /// 
        /// </summary> 
        /// <param name="source"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> source, Func<T, bool> whereClause = null)
        {
            if (whereClause == null) return source == null || !source.Any();
            return source == null || !source.Any(whereClause);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ICollection source)
        {
            return source == null || source.Count <= 0;
        }

        public static ICollection<T> ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
            return collection;
        }


        /// <summary>
        /// Obtains the data as a list; if it is *already* a list, the original object is returned without
        /// any duplication; otherwise, ToList() is invoked.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        /// <param name="source">The enumerable to return as a list.</param>
        public static List<T> AsList<T>(this IEnumerable<T> source) => (source == null || source is List<T>) ? (List<T>)source : source.ToList();


        public static bool ContainAnySameItem(this IEnumerable<string> a, IEnumerable<string> b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return a.Intersect(b).Any();
        }









    }
}
