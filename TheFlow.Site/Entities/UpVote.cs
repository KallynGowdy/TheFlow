using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for up votes.
    /// </summary>
    public class UpVote : Vote
    {
        /// <summary>
        /// Gets the string representation of the down vote.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} Up Voted the post by {1}", Voter.DisplayName, Post.Author.DisplayName);
        }
    }
}
