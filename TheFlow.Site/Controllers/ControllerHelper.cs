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
        public static string getAuthProvider(HttpRequestBase request)
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
        /// Authenticates the current user by either using forms authentication or repeated OpenID requests.
        /// If the user is not authenticated he/she will be redirected to their provider if the provider is supplied in the request.
        /// </summary>
        /// <returns>The currently authenticated user, or null if the user is not authenticated.</returns>
        public static User authenticate(HttpRequestBase request, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.First(a => a.OpenId == ((FormsIdentity)HttpContext.Current.User.Identity).Name);
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
                    string provider = getAuthProvider(request);
                    if (provider != null)
                    {
                        AuthenticationServer.Authenticate(request, provider);
                    }
                    return null;
                }
            }
            return null;
        }
    }
}