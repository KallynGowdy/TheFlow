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
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [MaxLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int? Age
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the user.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the reputation of the user.
        /// This attribute is NOT updated in the database.
        /// </summary>
        public int Reputation { get; set; }
    }
}