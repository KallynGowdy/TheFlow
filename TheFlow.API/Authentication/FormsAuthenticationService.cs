using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TheFlow.API.Authentication
{
    public class FormsAuthenticationService : IFormsAuthentication
    {
        /// <summary>
        /// Signs the current user in by giving the user an authentication cookie.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="createPersistantCookie"></param>
        public void SignIn(string userName, bool createPersistantCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistantCookie);
        }

        /// <summary>
        /// Signs the current user out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}