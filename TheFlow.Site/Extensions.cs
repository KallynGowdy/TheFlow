using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFlow.Site
{
    /// <summary>
    /// Defines a static class with helper extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Throws an Argument null exception if the current object is null. Returns the value of the object if not null.
        /// </summary>
        /// <param name="obj"></param>
        public static T ThrowIfNull<T>(this T obj, string argName = null)
        {
            if (obj == null)
            {
                if (argName != null)
                {
                    throw new ArgumentNullException(argName);
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            else
            {
                return obj;
            }
        }
    }
}