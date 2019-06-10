using System;
using System.Text;

namespace DotNetHelper.ObjectToSql.Extension
{
    internal static class StringBuilderExtension
    {
        public static StringBuilder ReplaceFirstOccurrence(this StringBuilder source, string find, string replace, StringComparison comparison)
        {
            if (source == null || source.Length <= 0 || string.IsNullOrEmpty(find))
                return source;
            var place = source.ToString().IndexOf(find, comparison);
            if (place == -1)
                return source;
            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static StringBuilder ReplaceLastOccurrence(this StringBuilder source, string find, string replace, StringComparison comparison)
        {
            if (source == null || source.Length <= 0 || string.IsNullOrEmpty(find))
                return source;
            var place = source.ToString().LastIndexOf(find, comparison);
            if (place == -1)
                return source;
            source = source.Remove(place, find.Length).Insert(place, replace);
            return source;
        }
    }
}
