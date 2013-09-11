// Copyright 2013 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheFlow.Api.Models;
using TheFlow.Api.Authentication;

namespace TheFlow.Site.Controllers
{
    public class MenuItem
    {
        /// <summary>
        /// Gets the content that this menu item should display.
        /// </summary>
        public string Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the link that this menu item goes to.
        /// </summary>
        public string Link
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the children of this menu item.
        /// </summary>
        public IList<MenuItem> Children
        {
            get;
            private set;
        }

        public MenuItem(string content, string link, IList<MenuItem> children = null)
        {
            this.Content = content;
            this.Link = link;
            if (children == null)
            {
                this.Children = new List<MenuItem>();
            }
            else
            {
                this.Children = children;
            }
        }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Returns menu items for the navigation bar depending on whether the user is logged in or not.
        /// </summary>
        /// <returns></returns>
        public ActionResult _Menu()
        {
            List<MenuItem> menu = new List<MenuItem>();
            TheFlow.Api.Entities.User user = ControllerHelper.Authenticate(Request);
            if (user == null)
            {
                menu.Add(new MenuItem("Create Account", "~/Users/LogIn"));
                menu.Add(new MenuItem("Log In", "~/Users/LogIn"));
            }
            else
            {
                menu.Add(new MenuItem(user.DisplayName, "~/Account", new List<MenuItem>(new []{new MenuItem("Log Out", "~/Users/LogOut")})));
                menu.Add(new MenuItem(user.Reputation.ToString(), "~/Account/Reputation"));
            }
            return PartialView(menu);
        }
    }
}
