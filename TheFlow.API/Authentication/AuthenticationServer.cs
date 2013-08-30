﻿// Copyright 2013 Kallyn Gowdy
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

using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using TheFlow.API.Entities;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TheFlow.API.Authentication
{
    /// <summary>
    /// Defines a static class that can be used for OpenID authentication.
    /// </summary>
    public static class AuthenticationServer
    {
        public const string OpenIdHeader = "OpenIdProvider";

        /// <summary>
        /// Gets the current user that is authenticated user Forms Authentication.
        /// Returns null if the user is not authenticated.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static User GetFormsAuthenticatedUser(IDbContext dataContext = null)
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
            return null;
        }

        /// <summary>
        /// Authenticates the user using OpenID given the current http request, the url of the OpenID provider, and the database context.
        /// </summary>
        /// <param name="httpRequest">The current incomming request.</param>
        /// <param name="providerUrl">The URL of the OpenID provider.</param>
        /// <param name="dataContext">The database context.</param>
        /// <returns>The user that is authenticated, otherwise null.</returns>
        public static void AuthenticateRedirect(HttpRequestBase httpRequest, string providerUrl, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (providerUrl == null)
            {
                IEnumerable<string> values = httpRequest.Headers.GetValues(OpenIdHeader);
                if (values != null)
                {
                    providerUrl = values.FirstOrDefault();

                    if (providerUrl == null)
                    {
                        foreach (string key in httpRequest.QueryString.Keys)
                        {
                            if (key.Equals(OpenIdHeader, StringComparison.OrdinalIgnoreCase))
                            {
                                providerUrl = httpRequest.QueryString[key];
                                break;
                            }
                        }
                    }
                }
            }

            if (providerUrl == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
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
                    request.AddExtension(fr);

                    request.RedirectToProvider();
                }
            }
            catch (ProtocolException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        /// <summary>
        /// Authenticates the user using OpenID given the current http request, the url of the OpenID provider, and the database context.
        /// </summary>
        /// <param name="httpRequest">The current incomming request.</param>
        /// <param name="providerUrl">The URL of the OpenID provider.</param>
        /// <param name="dataContext">The database context.</param>
        /// <returns>The user that is authenticated, otherwise null.</returns>
        public static ActionResult AuthenticateActionResult(HttpRequestBase httpRequest, string providerUrl, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (providerUrl == null)
            {
                IEnumerable<string> values = httpRequest.Headers.GetValues(OpenIdHeader);

                providerUrl = values.FirstOrDefault();

                if (providerUrl == null)
                {
                    foreach (string key in httpRequest.QueryString.Keys)
                    {
                        if (key.Equals(OpenIdHeader, StringComparison.OrdinalIgnoreCase))
                        {
                            providerUrl = httpRequest.QueryString[key];
                            break;
                        }
                    }
                }
            }

            if (providerUrl == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
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

                    request.AddExtension(fr);
                    return request.RedirectingResponse.AsActionResult();
                }
            }
            catch (ProtocolException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        /// <summary>
        /// Authenticates the user using OpenID given the current http request, the url of the OpenID provider, and the database context.
        /// </summary>
        /// <param name="httpRequest">The current incomming request.</param>
        /// <param name="providerUrl">The URL of the OpenID provider.</param>
        /// <param name="dataContext">The database context.</param>
        /// <returns>The user that is authenticated, otherwise null.</returns>
        public static User Authenticate(HttpRequestBase httpRequest, string providerUrl, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (providerUrl == null)
            {
                IEnumerable<string> values = httpRequest.Headers.GetValues(OpenIdHeader);

                providerUrl = values.FirstOrDefault();

                if (providerUrl == null)
                {
                    foreach (string key in httpRequest.QueryString.Keys)
                    {
                        if (key.Equals(OpenIdHeader, StringComparison.OrdinalIgnoreCase))
                        {
                            providerUrl = httpRequest.QueryString[key];
                            break;
                        }
                    }
                }
            }

            if (providerUrl == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            User user;
            if (IsAuthenticated(out user, dataContext))
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

                    request.AddExtension(fr);
                    request.RedirectToProvider();
                }
            }
            catch (ProtocolException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return null;
        }

        /// <summary>
        /// Authenticates the user using OpenID given the current http request, the url of the OpenID provider, and the database context.
        /// </summary>
        /// <param name="httpRequest">The current incomming request.</param>
        /// <param name="providerUrl">The URL of the OpenID provider.</param>
        /// <param name="dataContext">The database context.</param>
        /// <returns>The user that is authenticated, otherwise null.</returns>
        public static User Authenticate(HttpRequestMessage httpRequest, string providerUrl, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            if (providerUrl == null)
            {
                IEnumerable<string> values;
                if (httpRequest.Headers.TryGetValues(OpenIdHeader, out values))
                {
                    providerUrl = values.FirstOrDefault();
                }

                if (providerUrl == null)
                {
                    KeyValuePair<string, string> query = httpRequest.GetQueryNameValuePairs().FirstOrDefault(a => a.Key == OpenIdHeader);
                    if (query.Key != null)
                    {
                        providerUrl = query.Value;
                    }
                }
            }

            if (providerUrl == null)
            {
                throw new HttpResponseException(httpRequest.CreateResponse(HttpStatusCode.BadRequest, string.Format("The URL of the OpenId provider endpoint must be in either the '{0}' header or query string ('{0}'={{url}})", OpenIdHeader)));
            }

            User user;
            if (IsAuthenticated(out user, dataContext))
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

                    request.AddExtension(fr);
                    request.RedirectToProvider();
                }
            }
            catch (ProtocolException e)
            {
                throw new HttpResponseException(httpRequest.CreateResponse(HttpStatusCode.BadRequest, new { Error = string.Format("{0}: {1}", providerUrl, e.Message) }));
            }
            return null;
        }

        /// <summary>
        /// Determines if the user is authenticated.
        /// </summary>
        /// <param name="user">The user that is authenticated. Null if the user is not authenticated.</param>
        /// <param name="dataContext">The current DB context to use.</param>
        /// <returns></returns>
        public static bool IsAuthenticated(out User user, DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = new DbContext();
            }

            using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
            {
                IAuthenticationResponse response = openId.GetResponse();
                if (response != null)
                {
                    if (response.Status == AuthenticationStatus.Authenticated)
                    {
                        FetchResponse claims = response.GetExtension<FetchResponse>();
                        string claimedIdentifier = response.ClaimedIdentifier.ToString();
                        if (dataContext.Users.All(u => u.OpenId != claimedIdentifier))
                        {
                            if (claims != null)
                            {
                                user = new User
                                {
                                    OpenId = claimedIdentifier,
                                    EmailAddress = claims.Attributes[WellKnownAttributes.Contact.Email].Values.First(),
                                    FirstName = claims.Attributes[WellKnownAttributes.Name.First].Values.First(),
                                    LastName = claims.Attributes[WellKnownAttributes.Name.Last].Values.First(),
                                    Preferences = new Preferences()
                                };
                            }
                            else
                            {
                                user = new User
                                {
                                    OpenId = claimedIdentifier
                                };
                            }
                        }
                        else
                        {
                            user = dataContext.Users.First(u => u.OpenId == claimedIdentifier);
                        }
                        return true;
                    }
                }
            }
            user = null;
            return false;
        }

        /// <summary>
        /// Determines if the user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
            {
                IAuthenticationResponse response = openId.GetResponse();
                if (response != null)
                {
                    if (response.Status == AuthenticationStatus.Authenticated)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the currently authenticated user, returns null if no user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static User GetAuthenticatedUser(DbContext dataContext = null)
        {
            if (dataContext == null)
            {
                dataContext = (DbContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(DbContext));
            }

            using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
            {
                IAuthenticationResponse response = openId.GetResponse();
                if (response != null)
                {
                    if (response.Status == AuthenticationStatus.Authenticated)
                    {
                        FetchResponse claims = response.GetExtension<FetchResponse>();
                        string claimedIdentifier = response.ClaimedIdentifier.ToString();
                        if (dataContext.Users.All(u => !u.OpenId.Equals(claimedIdentifier, StringComparison.Ordinal)))
                        {
                            if (claims != null)
                            {
                                return new User
                                {
                                    OpenId = claimedIdentifier,
                                    EmailAddress = claims.Attributes[WellKnownAttributes.Contact.Email].Values.First(),
                                    FirstName = claims.Attributes[WellKnownAttributes.Name.First].Values.First(),
                                    LastName = claims.Attributes[WellKnownAttributes.Name.Last].Values.First()
                                };
                            }
                            else
                            {
                                return new User
                                {
                                    OpenId = claimedIdentifier
                                };
                            }
                        }
                        else
                        {
                            return dataContext.Users.First(u => u.OpenId.Equals(claimedIdentifier, StringComparison.Ordinal));
                        }
                    }
                }
            }
            return null;
        }
    }
}