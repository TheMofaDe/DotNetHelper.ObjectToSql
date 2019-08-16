using System;
using System.Collections.Generic;
using DotNetHelper.ObjectToSql.Exceptions;

namespace DotNetHelper.ObjectToSql.Extension
{
    internal static class ObjectExtension
    {

        public static void IsNullThrow(this object obj, string name, Exception error = null)
        {
            if (obj != null) return;
            if (error == null) error = new ArgumentNullException(name);
            throw error;
        }

        public static void IsEmptyThrow<T>(this IEnumerable<T> obj, string name, Exception error = null)
        {

            if (obj == null)
            {
                if (error == null) error = new ArgumentNullException(name);
                throw error;
            }

            if (obj.AsList().Count > 0) return;
            if (error == null) error = new EmptyArgumentException($"The argument {name} was empty");
            throw error;
        }





    }
}
