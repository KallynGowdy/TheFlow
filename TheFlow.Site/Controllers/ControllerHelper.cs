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
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using TheFlow.API.Authentication;
using TheFlow.API.Entities;

namespace TheFlow.Site.Controllers
{
    public static class ControllerHelper
    {
        /// <summary>
        /// Gets the OpenID Authentication provider by searching the form, headers, query string and then cookies.
        /// Returns null if the url could not be found.
        /// </summary>
        /// <returns></returns>
        public static string GetAuthProvider(HttpRequestBase request)
        {
            //check the form
            string url = request.Form["OpenIdProvider"];
            if (url == null)
            {
                //check the headers
                url = request.Headers["OpenIdProvider"];
                if (url == null)
                {
                    //check the query string
                    url = request.QueryString["OpenIdProvider"];
                    if (url == null)
                    {
                        //Check the cookies
                        HttpCookie cookie = request.Cookies["TheFlow-OpenIdProvider"];
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
        /// Determines if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity;
        }

        /// <summary>
        /// Authenticates the current user by either using forms authentication or repeated OpenID requests.
        /// If the user is not authenticated he/she will be redirected to their provider if the provider is supplied in the request.
        /// </summary>
        /// <returns>The currently authenticated user, or null if the user is not authenticated.</returns>
        public static User Authenticate(HttpRequestBase request, IDbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.SingleOrDefault(a => a.OpenId == ((FormsIdentity)HttpContext.Current.User.Identity).Name);
                return user;
            }
            else if (!FormsAuthentication.IsEnabled)
            {
                User user;
                if (AuthenticationServer.IsAuthenticated(out user))
                {
                    return user;
                }
                else
                {
                    string provider = GetAuthProvider(request);
                    if (provider != null)
                    {
                        AuthenticationServer.Authenticate(request, provider);
                    }
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the cookie with the given name from the users cookie collection.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static bool RemoveCookie(HttpRequestBase request, HttpResponseBase response, string cookieName)
        {
            if (request.Cookies[cookieName] != null)
            {
                HttpCookie newCookie = new HttpCookie(cookieName);
                newCookie.Expires = DateTime.UtcNow.AddDays(-1);
                response.Cookies.Add(newCookie);
                return true;
            }
            return false;
        }
    }
}