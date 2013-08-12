using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Cryptography;
namespace TheFlow.API.Membership
{
    public class AccountMembershipService : IMembershipProvider
    {
        private MembershipProvider provider;

        public AccountMembershipService()
            : this(null)
        {

        }

        public AccountMembershipService(MembershipProvider provider)
        {
            this.provider = provider ?? System.Web.Security.Membership.Provider;
        }

        /// <summary>
        /// Generates a new completely 50-byte character password.
        /// </summary>
        /// <returns></returns>
        public static string GeneratePassword()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] pass = new byte[50];
                rng.GetBytes(pass);
                return Convert.ToBase64String(pass);
            }
        }

        public System.Web.Security.MembershipCreateStatus CreateUser(string username, string email)
        {
            MembershipCreateStatus status;
            string password = GeneratePassword();
            this.provider.CreateUser(username, password, email, null, null, true, null, out status);
            return status;
        }
    }
}