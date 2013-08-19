using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TheFlow.Site
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a link inside of a form that contains a validation token.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string LinkForm(this HtmlHelper html, string text, string action, string controller)
        {
            return string.Format("<form action=\"{0}\" method=\"get\"><a onclick=\"$(this).parent().submit()\">{1}</a></form>", string.Format("~/{0}/{1}", controller, action), text);
        }
    }
}