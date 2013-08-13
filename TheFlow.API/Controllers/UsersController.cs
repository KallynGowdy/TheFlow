using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using TheFlow.API.Authentication;
using TheFlow.API.Entities;
using TheFlow.API.Models;
//using TheFlow.API.Authentication;
//using TheFlow.API.Membership;

namespace TheFlow.API.Controllers
{
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

        public UsersController(DbContext context)
        {
            DataContext = context;
        }

        /// <summary>
        /// Gets the database context for this controller.
        /// </summary>
        public DbContext DataContext
        {
            get;
            private set;
        }

        /// <summary>
        /// Causes the OpenID redirect to the given OpenID provider URL.
        /// </summary>
        /// <param name="providerUrl">The URL of the OpenID provider to redirect to.</param>
        [HttpGet]
        public void LogIn(string providerUrl)
        {
            User user = AuthenticationServer.Authenticate(Request, providerUrl, DataContext);
            if (!DataContext.Users.Any(u => u.OpenId == user.OpenId))
            {
                if (user.DisplayName == null)
                {
                    user.DisplayName = generateDisplayName();
                }
                DataContext.Users.Add(user);
                DataContext.SaveChanges();
            }
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, user.EmailAddress));
        }

        /// <summary>
        /// Updates the logged in user's information based on the given model.
        /// </summary>
        /// <param name="updatedModel">The user model that contains information that the user profile should be updated to.</param>
        [HttpPut]
        public void UpdateUser([FromBody]UserModel updatedModel)
        {
            User user = AuthenticationServer.Authenticate(Request, Request.Headers.GetValues("OpenIdProvider").First(), DataContext);
            if (user != null)
            {
                if (updatedModel.DisplayName != null)
                {
                    user.DisplayName = updatedModel.DisplayName;
                }
                if (updatedModel.Age != null)
                {
                    user.Age = updatedModel.Age.Value;
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
        [AllowAnonymous]
        public IEnumerable<User> GetUsers([FromUri]int maxCount = 50)
        {
            return DataContext.Users.Take(maxCount).OrderBy(u => u.Reputation);
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
