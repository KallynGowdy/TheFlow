using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for a vote.
    /// </summary>
    public abstract class Vote
    {
        /// <summary>
        /// Gets or set the user that voted.
        /// </summary>
        [Required]
        public virtual User Voter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the user voted. required.
        /// </summary>
        [Required]
        public DateTime? DateVoted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post that was voted on.
        /// </summary>
        [Required]
        public virtual Post Post
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Id of the vote.
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }
    }
}