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
using System.Web.Mvc;
using System.Web.Security;
using TheFlow.Api.Authentication;
using TheFlow.Api.Entities;
using TheFlow.Api.Models;

namespace TheFlow
{
    namespace Site.Controllers
    {

        public class UsersController : Controller
        {
            DbContext dataContext = new DbContext();

            protected override void Initialize(System.Web.Routing.RequestContext requestContext)
            {
                base.Initialize(requestContext);
            }

            /// <summary>
            /// Gets the info on the requested user.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public ActionResult Info(string user)
            {
                User u = null;
                if (user != null)
                {
                    u = dataContext.Users.FirstOrDefault(a => a.DisplayName == user);
                }
                else
                {
                    u = authenticate();
                }
                if (u == null)
                {
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                }
                else
                {
                    return View(u);
                }
            }

            /// <summary>
            /// Serves the edit view to the authenticated user.
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            [Authorize]
            public ActionResult Edit()
            {
                User u = authenticate();

                PreferencesModel prefs;
                if (u.Preferences == null)
                {
                    u.Preferences = new Preferences();
                }
                prefs = u.Preferences.ToModel();

                return View(new UserModel
                            {
                                DateOfBirth = u.DateOfBirth,
                                DisplayName = u.DisplayName,
                                EmailAddress = u.EmailAddress,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Location = u.Location,
                                Preferences = prefs
                            });
            }

            [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
            [ValidateAntiForgeryToken]
            [ValidateInput(true)]
            public ActionResult Edit(UserModel newInfo)
            {
                User user = authenticate(ref newInfo);
                if (newInfo != null)
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
                    if (newInfo.Preferences != null && newInfo.Preferences.CodeTheme.HasValue)
                    {
                        if (user.Preferences == null)
                        {
                            user.Preferences = new Preferences { CodeStyle = newInfo.Preferences.CodeTheme.Value };
                        }
                        else
                        {
                            user.Preferences.CodeStyle = newInfo.Preferences.CodeTheme.Value;
                        }
                    }
                    dataContext.SaveChanges();
                }
                return View("Info", user);
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
                return ControllerHelper.RedirectBack(Request, Redirect, true);
            }

            /// <summary>
            /// Checks if the current user is authenticated by their provider and then either redirects the user to the original page or the login page.
            /// </summary>
            /// <returns></returns>
            [System.Web.Mvc.AcceptVerbs(HttpVerbs.Get)]
            public ActionResult LogIn()
            {
                //FormsAuthentication.SetAuthCookie("https://www.google.com/accounts/o8/id?id=AItOawmiaM32tj_WjvFmiMV0PatVe4Spployfc0", false);


                string returnUrl = Request["ReturnUrl"];
                if (returnUrl == null)
                {
                    returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : null;
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
            public ActionResult LogIn([System.Web.Http.FromUri] string OpenIdProvider)
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
                    catch (System.Web.Http.HttpResponseException)
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

    namespace Api.Controllers
    {
        using DotNetOpenAuth.Messaging;
        using DotNetOpenAuth.OpenId;
        using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
        using DotNetOpenAuth.OpenId.RelyingParty;
        using System.Web.Http;
        using TheFlow.Api.Authorization;

        /// <summary>
        /// Defines an enum that is used for determining which attributes to sort users by.
        /// </summary>
        public enum UserSortFilter
        {
            Reputation,
            NewUsers,
            Voters,
            Editors,
            Moderators
        }

        /// <summary>
        /// Defines a controller that manages interaction with users.
        /// </summary>
        public class UsersController : ApiController
        {
            public UsersController(IDbContext context)
            {
                DataContext = context;
            }

            /// <summary>
            /// Gets the database context for this controller.
            /// </summary>
            public IDbContext DataContext
            {
                get;
                private set;
            }

            ///// <summary>
            ///// Causes the OpenID redirect to the given OpenID provider URL.
            ///// </summary>
            ///// <param name="providerUrl">The URL of the OpenID provider to redirect to.</param>
            //[HttpGet]
            //public void LogIn(string providerUrl)
            //{
            //    User user = AuthenticationServer.Authenticate(Request, providerUrl, DataContext);
            //    if (!DataContext.Users.Any(u => u.OpenId == user.OpenId))
            //    {
            //        if (user.DisplayName == null)
            //        {
            //            user.DisplayName = generateDisplayName();
            //        }
            //        DataContext.Users.Add(user);
            //        DataContext.SaveChanges();
            //    }
            //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, user.EmailAddress));
            //}

            [AllowAnonymous]
            public UserModel GetUser(string displayName)
            {
                return DataContext.Users.FirstOrDefault(a => a.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)).ToModel();
            }

            /// <summary>
            /// Creates a new user by authenticating using OpenId and the given model to add information.
            /// </summary>
            /// <param name="newUser"></param>
            [HttpPost]
            [OpenIDAuthorize(false, true)]
            public void CreateUser([FromBody]UserModel newUser)
            {
                User user = AuthenticationServer.GetAuthenticatedUser(DataContext);
                if (!DataContext.Users.Contains(user))
                {
                    DataContext.Users.Add(user);
                    if (user != null && newUser != null)
                    {
                        if (newUser.DisplayName != null)
                        {
                            user.DisplayName = newUser.DisplayName;
                        }
                        if (newUser.DateOfBirth != null)
                        {
                            user.DateOfBirth = newUser.DateOfBirth.Value;
                        }
                        if (newUser.EmailAddress != null)
                        {
                            user.EmailAddress = newUser.EmailAddress;
                        }
                        if (newUser.FirstName != null)
                        {
                            user.FirstName = newUser.FirstName;
                        }
                        if (newUser.LastName != null)
                        {
                            user.LastName = newUser.LastName;
                        }
                        if (newUser.Location != null)
                        {
                            user.Location = newUser.Location;
                        }
                        DataContext.SaveChanges();
                    }
                }
            }

            /// <summary>
            /// Updates the logged in user's information based on the given model.
            /// </summary>
            /// <param name="updatedModel">The user model that contains information that the user profile should be updated to.</param>
            [HttpPut]
            [OpenIDAuthorize(true, true)]
            public void UpdateUser([FromBody]UserModel updatedModel)
            {
                User user = AuthenticationServer.GetAuthenticatedUser(DataContext);
                if (user != null)
                {
                    if (updatedModel.DisplayName != null)
                    {
                        user.DisplayName = updatedModel.DisplayName;
                    }
                    if (updatedModel.DateOfBirth != null)
                    {
                        user.DateOfBirth = updatedModel.DateOfBirth.Value;
                    }
                    if (updatedModel.EmailAddress != null)
                    {
                        user.EmailAddress = updatedModel.EmailAddress;
                    }
                    if (updatedModel.FirstName != null)
                    {
                        user.FirstName = updatedModel.FirstName;
                    }
                    if (updatedModel.LastName != null)
                    {
                        user.LastName = updatedModel.LastName;
                    }
                    if (updatedModel.Location != null)
                    {
                        user.Location = updatedModel.Location;
                    }
                    DataContext.SaveChanges();
                }
            }

            /// <summary>
            /// Gets a collection of users sorted by reputation.
            /// </summary>
            /// <param name="maxCount">The maximum number of users to return, default is 50.</param>
            /// <returns>A collection of users.</returns>
            public IEnumerable<UserModel> GetUsers([FromUri]int maxCount = 50)
            {
                return DataContext.Users.Where(u => u != null).Take(maxCount).OrderBy(u => u.Reputation).Select(a =>
                    new UserModel
                    {
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Location = a.Location,
                        DisplayName = a.DisplayName,
                        DateOfBirth = a.DateOfBirth
                    });
            }

            /// <summary>
            /// Causes the current user to authenticate and returns their profile. Returns null if the user is not authenticated.
            /// </summary>
            /// <param name="providerUrl"></param>
            /// <returns></returns>
            User authenticate(string providerUrl)
            {
                User user;
                if (isAuthenticated(out user))
                {
                    return user;
                }
                if (!Identifier.IsValid(providerUrl))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                try
                {
                    using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                        IAuthenticationRequest request = openId.CreateRequest(providerUrl);

                        var fr = new FetchRequest();
                        fr.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
                        fr.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
                        fr.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));

                        //request.Mode = AuthenticationRequestMode.Setup;

                        //request the email and timezone
                        //request.AddExtension(new ClaimsRequest
                        //{
                        //    Email = DemandLevel.Require,
                        //    FullName = DemandLevel.Require,
                        //    Nickname = DemandLevel.Require,
                        //});
                        request.AddExtension(fr);
                        request.RedirectToProvider();
                    }
                }
                catch (ProtocolException e)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = string.Format("{0}: {1}", providerUrl, e.Message) }));
                }
                return null;
            }

            /// <summary>
            /// Determines if the user is authenticated.
            /// </summary>
            /// <param name="user">The user that is authenticated. Null if the user is not authenticated.</param>
            /// <returns></returns>
            bool isAuthenticated(out User user)
            {
                using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
                {
                    IAuthenticationResponse response = openId.GetResponse();
                    if (response != null)
                    {
                        if (response.Status == AuthenticationStatus.Authenticated)
                        {
                            FetchResponse claims = response.GetExtension<FetchResponse>();
                            string claimedIdentifier = response.ClaimedIdentifier.ToString();
                            if (DataContext.Users.All(u => u.OpenId != claimedIdentifier))
                            {
                                if (claims != null)
                                {
                                    //string fullname = string.Format("{0} {1}", claims.Attributes[WellKnownAttributes.Name.First].Values.First(), claims.Attributes[WellKnownAttributes.Name.Last].Values.First());
                                    user = DataContext.Users.Add(new User
                                    {
                                        OpenId = claimedIdentifier,
                                        EmailAddress = claims.Attributes[WellKnownAttributes.Contact.Email].Values.First(),
                                        FirstName = claims.Attributes[WellKnownAttributes.Name.First].Values.First(),
                                        LastName = claims.Attributes[WellKnownAttributes.Name.Last].Values.First(),
                                        DisplayName = generateDisplayName()
                                    });
                                }
                                else
                                {
                                    user = DataContext.Users.Add(new User
                                    {
                                        OpenId = claimedIdentifier
                                    });
                                }
                                DataContext.SaveChanges();
                            }
                            else
                            {
                                user = DataContext.Users.First(u => u.OpenId == claimedIdentifier);
                            }
                            return true;
                        }
                    }
                }
                user = null;
                return false;
            }

            /// <summary>
            /// Generates a new random display name based on the prefix in the settings and a random number.
            /// </summary>
            /// <returns></returns>
            private string generateDisplayName()
            {
                return ("User") + (DataContext.Users.Count() + 1).ToString();
            }

            ~UsersController()
            {
                if (DataContext != null)
                {
                    DataContext.Dispose();
                }
            }
        }
    }
}