using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFlow.Api.Entities;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for a down vote.
    /// </summary>
    public class ViewDownVoteModel : ViewVoteModel
    {
        public ViewDownVoteModel(DownVote vote)
            : base(vote)
        { }
    }
}