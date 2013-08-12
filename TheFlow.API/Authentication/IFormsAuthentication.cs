using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFlow.API.Authentication
{
    /// <summary>
    /// Defines an interface used for signing users in and out.
    /// </summary>
    public interface IFormsAuthentication
    {
        /// <summary>
        /// Signs the user with the given username in by creating an authentication cookie that is used with forms auth.
        /// </summary>
        /// <param name="userName">The username of the user to sign in.</param>
        /// <param name="createPersistantCookie">Whether the cookie should be persistance across sessions.</param>
        void SignIn(string userName, bool createPersistantCookie);

        /// <summary>
        /// Signs the current user out.
        /// </summary>
        void SignOut();
    }
}
