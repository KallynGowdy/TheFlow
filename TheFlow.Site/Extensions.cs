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
        /// Sanitizes the current html string and returns a new sanitized html string.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlString Sanitize(this HtmlString html)
        {
            return new HtmlString(Controllers.ControllerHelper.HtmlSanitizer.GetHtml(html.ToString()));
        }

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

        /// <summary>
        /// Converts the current string into an html string.
        /// </summary>
        /// <param name="obj">The string to display as html.</param>
        /// <returns></returns>
        public static HtmlString ToHtml(this string obj)
        {
            return new HtmlString(obj);
        }

        /// <summary>
        /// Gets the string representation of the current DateTime object formatted as a UTC string.
        /// </summary>
        /// <param name="date">The DateTime object to format.</param>
        /// <returns></returns>
        public static string ToUtcString(this DateTime date)
        {
            return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}