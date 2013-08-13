using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the real name of the user.
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the full name of the user.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the open id that this user owns.
        /// </summary>
        [Key]
        public string OpenId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the user.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public byte? Age { get; set; }

        /// <summary>
        /// Gets or sets the reputation that the user has.
        /// </summary>
        public uint Reputation { get; set; }

        /// <summary>
        /// Gets or sets the date that the user joined 'TheFlow'.
        /// </summary>
        public DateTime DateJoined { get; set; }

        public User()
        {
            DateJoined = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the collection of starred questions that this user has.
        /// </summary>
        public virtual ICollection<Star> StarredQuestions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of answers that this user has posted.
        /// </summary>
        public virtual ICollection<Answer> Answers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of questions that this user has asked.
        /// </summary>
        public virtual ICollection<Question> Questions
        {
            get;
            set;
        }
    }
}
