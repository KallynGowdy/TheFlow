using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for a user.
    /// Used for network communications(i.e. communications with the API).
    /// All attributes are optional.
    /// </summary>
    public class UserModel
    {
        [MaxLength(255)]
        public string DisplayName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public byte? Age { get; set; }

        public string Location { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}