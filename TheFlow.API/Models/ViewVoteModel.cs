using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFlow.API.Entities;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines an abstract model for votes.
    /// </summary>
    public abstract class ViewVoteModel
    {
        /// <summary>
        /// Gets or set the voter stored in this model.
        /// </summary>
        public UserModel Voter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the vote was made.
        /// </summary>
        public DateTime DateVoted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id stored in this model representing id of the vote entity.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        protected ViewVoteModel(Vote vote)
        {
            this.Voter = vote.Voter.ToModel();
            this.DateVoted = vote.DateVoted.Value;
            this.Id = vote.Id;
        }
    }
}