using System;

namespace DotNetHelper.ObjectToSql.Extension
{
   internal static class ExtString
    {


        public static string ReplaceFirstOccurrence(this string source, string find, string replace, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find))
                return source;
            var place = source.IndexOf(find, comparison);
            if (place == -1)
                return source;
            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace, StringComparison comparison)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find))
                return source;
            var place = source.LastIndexOf(find, comparison);
            if (place == -1)
                return source;
            source = source.Remove(place, find.Length).Insert(place, replace);
            return source;
        }



        

    }
}
