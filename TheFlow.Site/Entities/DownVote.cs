using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for down votes.
    /// </summary>
    public class DownVote : Vote
    {
        /// <summary>
        /// Gets the string representation of the down vote.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} Down Voted the post by {1}", Voter.DisplayName, Post.Author.DisplayName);
        }
    }
}
