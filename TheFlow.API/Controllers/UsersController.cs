using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using TheFlow.API.Authentication;
//using TheFlow.API.Membership;

namespace TheFlow.API.Controllers
{
    /// <summary>
    /// Defines a controller that manages interaction with users.
    /// </summary>
    public class UsersController : ApiController
    {

        ///// <summary>
        ///// Gets the forms authentication provider for this controller.
        ///// </summary>
        //public IFormsAuthentication FormsAuth
        //{
        //    get;
        //    private set;
        //}

        //public IMembershipProvider Provider
        //{
        //    get;
        //    private set;
        //}

        ///// <summary>
        ///// Creates a new controller given the authentication provider to use.
        ///// </summary>
        ///// <param name="auth"></param>
        //public UsersController(IFormsAuthentication auth, IMembershipProvider provider)
        //{
        //    this.FormsAuth = auth;
        //    this.Provider = provider;
        //}

        /// <summary>
        /// Causes the OpenID redirect to the given OpenID provider URL.
        /// </summary>
        /// <param name="providerUrl">The URL of the OpenID provider to redirect to.</param>
        [HttpGet]
        public void LogIn(string providerUrl)
        {
            ClaimsResponse claims;
            if (isAuthenticated(out claims))
            {
                if (claims != null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.OK, string.Format("Hello {0}!", claims.FullName != null ? claims.FullName : claims.Email)));
                }
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

                    //request the email and timezone
                    request.AddExtension(new ClaimsRequest
                    {
                        Email = DemandLevel.Require,
                        FullName = DemandLevel.Request
                    });

                    request.RedirectToProvider();
                }
            }
            catch (ProtocolException e)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = string.Format("{0}: {1}", providerUrl, e.Message) }));
            }
        }

        /// <summary>
        /// Determines if the user is authenticated.
        /// </summary>
        /// <param name="claims">The claims that will be returned by the OpenID authentication process if the user is authenticated.</param>
        /// <returns></returns>
        bool isAuthenticated(out ClaimsResponse claims)
        {
            using (OpenIdRelyingParty openId = new OpenIdRelyingParty())
            {
                IAuthenticationResponse response = openId.GetResponse();
                if (response != null)
                {
                    if (response.Status == AuthenticationStatus.Authenticated)
                    {
                        claims = response.GetExtension<ClaimsResponse>();
                        return true;
                    }
                }
            }
            claims = null;
            return false;
        }
    }
}
