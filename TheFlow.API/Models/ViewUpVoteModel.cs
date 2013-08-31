using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFlow.API.Entities;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for an up vote.
    /// </summary>
    public class ViewUpVoteModel : ViewVoteModel
    {
        public ViewUpVoteModel(UpVote vote)
            : base(vote)
        {
        }
    }
}