using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace TheFlow.API.Membership
{
    /// <summary>
    /// Defines a membership provider that allows creation of new users.
    /// </summary>
    public interface IMembershipProvider
    {
        /// <summary>
        /// Creates a new user with the given identifier and email address.
        /// </summary>
        /// <param name="claimedIdentifier">The username of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <returns></returns>
        MembershipCreateStatus CreateUser(string userName, string email);
    }
}
