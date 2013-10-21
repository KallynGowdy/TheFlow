using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;

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

        public static MvcHtmlString BootstrapValidation(this HtmlHelper html)
        {
            StringBuilder s = new StringBuilder();
            var errors = html.ViewData.ModelState.SelectMany(a => a.Value.Errors).ToArray();
            if (errors.Length > 1)
            {
                s.Append("<ul>");
                for (int i = 0; i < errors.Length; i++)
                {
                    s.AppendFormat("<li>{0}</li>", errors[i].ErrorMessage);
                }
                s.Append("</ul>");
            }
            else if (errors.Length == 1)
            {
                s.Append(errors[0].ErrorMessage);
            }
            return new MvcHtmlString(s.ToString());
        }
    }
}