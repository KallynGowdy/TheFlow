using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFlow.Site.HtmlSanitization;
using TheFlow.Site.Controllers;
using System.Diagnostics;
namespace TheFlow.Tests
{
    public class Tests
    {
        public void TestSanitization()
        {
            Debug.Assert(!ControllerHelper.HtmlSanitizer.GetHtml("This is malicious. <script>alert('hi!!');</script>").Contains("alert"));
            Debug.Assert(!ControllerHelper.HtmlSanitizer.GetHtml("This is not malicious, but the url is not acceptable. <a href=\"www.google.com\">Hi!!!</a>").Contains("<a href="));
            Debug.Assert(!ControllerHelper.HtmlSanitizer.GetHtml("This is malicious. <img src=javascript: alert('hi!! XSS!!')/>").Contains("img"));
            Debug.Assert(ControllerHelper.HtmlSanitizer.GetHtml("This is not malicious, but malformed. <p> paragraph!").Contains("<p>"));
            Debug.Assert(!ControllerHelper.HtmlSanitizer.GetHtml("This is malicious. <a onmouseover=alert(document.cookie)>Hi!!!</a>").Contains("<a onmouseover"));
        }
    }
}
