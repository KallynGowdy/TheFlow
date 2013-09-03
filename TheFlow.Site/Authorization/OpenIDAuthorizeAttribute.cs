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

using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TheFlow.Api.Entities;
using TheFlow.Api.Entities;

namespace TheFlow.Api.Authorization
{
    /// <summary>
    /// Defines an attribute that authorizes the user based on OpenID.
    /// </summary>
    public class OpenIDAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets whether ssl is required. (Default true)
        /// </summary>
        public bool RequireSsl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether to save newly authenticated users to the database.
        /// </summary>
        public bool SaveUsersToDatabase
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        public DbContext DataContext
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new OpenIDAuthorizeAttribute given whether to require SSL.
        /// </summary>
        /// <param name="requireSsl"></param>
        public OpenIDAuthorizeAttribute(bool requireSsl, bool saveNewUsers = false)
        {
            this.RequireSsl = requireSsl;
            this.SaveUsersToDatabase = saveNewUsers;
        }

        public OpenIDAuthorizeAttribute()
        {
            this.RequireSsl = true;
            DataContext = new DbContext();
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (RequireSsl && !actionContext.Request.RequestUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Https is required to access this resource");
                return;
            }

            IEnumerable<string> providers;
            if (actionContext.Request.Headers.TryGetValues("OpenIdProvider", out providers))
            {
                if (providers.Any())
                {
                    foreach (string provider in providers)
                    {
                        try
                        {
                            User user = authenticate(actionContext.Request, provider);
                            if (user != null)
                            {
                                return;
                            }
                            else
                            {
                                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Authorization has been denied for this request");
                            }
                        }
                        catch (HttpResponseException e)
                        {
                            if (provider == providers.Last())
                            {
                                actionContext.Response = e.Response;
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                KeyValuePair<string, string> provider = actionContext.Request.GetQueryNameValuePairs().FirstOrDefault(a => a.Key.Equals("openidprovider", StringComparison.OrdinalIgnoreCase));
                if (provider.Key != null)
                {
                    try
                    {
                        User user = authenticate(actionContext.Request, provider.Value);
                        if (user != null)
                        {
                            return;
                        }
                    }
                    catch (HttpResponseException e)
                    {
                        actionContext.Response = e.Response;
                        return;
                    }
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Error: you must provide a valid OpenID provider URL in the 'OpenIdProvider' header.");
        }

        /// <summary>
        /// Authenticates the user using OpenID given the current http request, the url of the OpenID provider, and the database context.
        /// </summary>
        /// <param name="httpRequest">The current incomming request.</param>
        /// <param name="providerUrl">The URL of the OpenID provider.</param>
        /// <param name="dataContext">The database context.</param>
        /// <returns>The user that is authenticated, otherwise null.</returns>
        private User authenticate(HttpRequestMessage httpRequest, string providerUrl)
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
        /// <returns></returns>
        private bool isAuthenticated(out User user)
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
                        if (DataContext.Users.All(u => !u.OpenId.Equals(response.ClaimedIdentifier, StringComparison.Ordinal)))
                        {
                            if (claims != null)
                            {
                                user = new User
                                {
                                    OpenId = claimedIdentifier,
                                    EmailAddress = claims.Attributes[WellKnownAttributes.Contact.Email].Values.First(),
                                    FirstName = claims.Attributes[WellKnownAttributes.Name.First].Values.First(),
                                    LastName = claims.Attributes[WellKnownAttributes.Name.Last].Values.First()
                                };
                                DataContext.Users.Add(user);
                            }
                            else
                            {
                                user = new User
                                {
                                    OpenId = claimedIdentifier
                                };
                            }
                            DataContext.SaveChanges();
                        }
                        else
                        {
                            user = DataContext.Users.First(u => u.OpenId.Equals(response.ClaimedIdentifier, StringComparison.Ordinal));
                        }
                        return true;
                    }
                }
            }
            user = null;
            return false;
        }
    }
}