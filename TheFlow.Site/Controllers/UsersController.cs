using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult LogIn()
        {
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
                        string provider = Request.Form["OpenIdProvider"];
                        if (provider != null)
                        {
                            Request.Cookies.Add(new HttpCookie("TheFlow-OpenIdProvider", provider));
                        }
                    }
                    return RedirectToAction("Welcome", "Users");
                }
            }
            else
            {
                string provider = Request.Form["OpenIdProvider"];
                if (provider != null)
                {
                    try
                    {
                        AuthenticationServer.Authenticate(Request, Request.Form["OpenIdProvider"]);
                    }
                    catch (HttpResponseException)
                    {
                        return View();
                    }
                }
            }

            return View();
        }


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
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated && User.Identity is FormsIdentity)
            {
                User user = dataContext.Users.First(a => a.OpenId == ((FormsIdentity)User.Identity).Name);
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
    }
}
