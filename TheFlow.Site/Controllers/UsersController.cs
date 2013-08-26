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
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using TheFlow.API.Authentication;
using TheFlow.API.Entities;
using TheFlow.API.Models;

namespace TheFlow.Site.Controllers
{
    public class UsersController : Controller
    {
        DbContext dataContext = new DbContext();

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        public ActionResult Info(string user)
        {
            return View(dataContext.Users.First(a => a.DisplayName == user).ToModel());
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult Edit(UserModel newInfo)
        {
            if (newInfo != null)
            {
                User user = authenticate(ref newInfo);
                if (user != null)
                {
                    if (newInfo.DisplayName != null)
                    {
                        user.DisplayName = newInfo.DisplayName;
                    }
                    if (newInfo.DateOfBirth != null)
                    {
                        user.DateOfBirth = newInfo.DateOfBirth;
                    }
                    if (newInfo.EmailAddress != null)
                    {
                        user.EmailAddress = newInfo.EmailAddress;
                    }
                    if (newInfo.Location != null)
                    {
                        user.Location = newInfo.Location;
                    }
                    if (newInfo.FirstName != null)
                    {
                        user.FirstName = newInfo.FirstName;
                    }
                    if (newInfo.LastName != null)
                    {
                        user.LastName = newInfo.LastName;
                    }
                    dataContext.SaveChanges();
                }
            }
            return View("Info", newInfo.DisplayName);
        }

        /// <summary>
        /// Logs the current user out.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            if (FormsAuthentication.IsEnabled)
            {
                FormsAuthentication.SignOut();
            }
            else
            {
                ControllerHelper.RemoveCookie(Request, Response, "TheFlow-OpenIdProvider");
            }
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }

        /// <summary>
        /// Checks if the current user is authenticated by their provider and then either redirects the user to the original page or the login page.
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogIn()
        {
            string returnUrl = Request["ReturnUrl"];
            if (returnUrl == null)
            {
                returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : null;
            }

            User user;
            if (AuthenticationServer.IsAuthenticated(out user))
            {
                if (user != null)
                {
                    if (!dataContext.Users.Any(a => a.OpenId == user.OpenId))
                    {
                        if (user.DisplayName == null)
                        {
                            user.DisplayName = string.Format("User{0}", dataContext.Users.Count() + 1);
                        }
                        user.DateJoined = DateTime.Now;
                        dataContext.Users.Add(user);
                        dataContext.SaveChanges();
                    }
                    bool rememberMe;
                    bool.TryParse(Request.Form["RememberMe"], out rememberMe);

                    if (FormsAuthentication.IsEnabled)
                    {
                        FormsAuthentication.SetAuthCookie(user.OpenId, rememberMe);
                    }
                    else
                    {
                        string provider = getAuthProvider();
                        if (provider != null)
                        {
                            //Add the provider to use for auth as a cookie
                            Response.Cookies.Add(new HttpCookie("TheFlow-OpenIdProvider", provider));
                        }
                    }
                }
                returnUrl = Request.Cookies["TheFlow-ReturnUrl"].Value;
                if (returnUrl != null)
                {
                    ControllerHelper.RemoveCookie(Request, Response, "TheFlow-ReturnUrl");
                    return Redirect(returnUrl);
                }
            }
            else
            {
                if (returnUrl != null)
                {
                    Response.Cookies.Add(new HttpCookie("TheFlow-ReturnUrl", returnUrl));
                }
            }
            return View();
        }

        /// <summary>
        /// Logs the user in using the provider url sent in the form.
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult LogIn([FromUri] string OpenIdProvider)
        {
            //Validate the request to make sure it is from our site.
            //This prevents Cross-Site Request Forgery.
            //DO NOT COMMENT OUT, OR REMOVE.
            AntiForgery.Validate();

            //Response.Cookies.Add(new HttpCookie("TheFlow-ReturnUrl", Request.UrlReferrer.AbsoluteUri));

            //Authenticate the user based on their OpenID provider.
            if (OpenIdProvider != null)
            {
                try
                {
                    if (!FormsAuthentication.IsEnabled)
                    {
                        //Add the provider to use for auth as a cookie
                        Response.Cookies.Add(new HttpCookie("TheFlow-OpenIdProvider", OpenIdProvider));
                    }
                    Response.BufferOutput = true;
                    AuthenticationServer.AuthenticateRedirect(Request, OpenIdProvider);
                    //return AuthenticationServer.AuthenticateActionResult(Request, OpenIdProvider);
                    return View();
                }
                catch (HttpResponseException)
                {
                    return View();
                }
            }

            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Welcome()
        {
            User user = authenticate();
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("LogIn", "Users");
        }

        /// <summary>
        /// Gets the OpenID Authentication provider by searching the form, headers, query string and then cookies.
        /// Returns null if the url could not be found.
        /// </summary>
        /// <returns></returns>
        string getAuthProvider()
        {
            //check the form
            string url = Request.Form["OpenIdProvider"];
            if (url == null)
            {
                //check the headers
                url = Request.Headers["OpenIdProvider"];
                if (url == null)
                {
                    //check the query string
                    url = Request.QueryString["OpenIdProvider"];
                    if (url == null)
                    {
                        //Check the cookies
                        HttpCookie cookie = Request.Cookies["TheFlow-OpenIdProvider"];
                        if (cookie != null)
                        {
                            url = cookie.Value;
                        }
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// Authenticates the current user by either using forms authentication or repeated OpenID requests.
        /// If the user is not authenticated he/she will be redirected to their provider if the provider is supplied in the request.
        /// </summary>
        /// <returns>The currently authenticated user, or null if the user is not authenticated.</returns>
        User authenticate()
        {
            //Check for forms auth first
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated && User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.First(a => a.OpenId == ((FormsIdentity)User.Identity).Name);
                return user;
            }
            //Otherwise default to OpenID authentication
            else if (!FormsAuthentication.IsEnabled)
            {
                User user;
                if (AuthenticationServer.IsAuthenticated(out user))
                {
                    return user;
                }
                else
                {
                    string provider = getAuthProvider();
                    if (provider != null)
                    {
                        AuthenticationServer.Authenticate(Request, provider);
                    }
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Authenticates the current user by either using forms authentication or repeated OpenID requests.
        /// If the user is not authenticated he/she will be redirected to their provider if the provider is supplied in the request.
        /// </summary>
        /// <param name="extraData">The extra data to preserve in a request, only used for OpenID authentication to preserve POST request data.</param>
        /// <returns>The currently authenticated user, or null if the user is not authenticated.</returns>
        User authenticate<T>(ref T extraData) where T : class
        {
            //Check for forms auth first
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated && User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.First(a => a.OpenId == ((FormsIdentity)User.Identity).Name);
                return user;
            }
            ////Otherwise default to OpenID authentication
            //else if (!FormsAuthentication.IsEnabled)
            //{
            //    User user;
            //    if (AuthenticationServer.IsAuthenticated(out user))
            //    {
            //        string extra = Request.Headers["TheFlow-ExtraData"];
            //        if (extra != null)
            //        {
            //            extraData = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Convert.FromBase64String(extra)));
            //        }
            //        return user;
            //    }
            //    else
            //    {
            //        if (extraData != null)
            //        {
            //            string extra = Convert.ToBase64String(Encoding.UTF8.GetBytes(Json(extraData).ToString()));
            //            Response.Headers["TheFlow-ExtraData"] = extra;
            //        }

            //        string provider = getAuthProvider();
            //        if (provider != null)
            //        {
            //            AuthenticationServer.Authenticate(Request, provider);
            //        }
            //        return null;
            //    }
            //}
            return null;
        }
    }
}
