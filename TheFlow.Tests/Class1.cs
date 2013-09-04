using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFlow.Site.HtmlSanitization;
using TheFlow.Site.Controllers;
using System.Diagnostics;
using TheFlow.Api.Entities;
using System.Data.Common;
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

        public void TestGetUsers()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (TheFlow.Api.Entities.DbContext context = new TheFlow.Api.Entities.DbContext(connection))
            {
                context.Users.Add(new User
                {
                    DisplayName = "Person",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    DateJoined = DateTime.Now,
                    Preferences = new Preferences()
                });
                Debug.Assert(context.Users.ToArray().Length > 0);
            }
        }
    }
}
