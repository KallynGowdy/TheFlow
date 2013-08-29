using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFlow.API.Entities;
using TheFlow.API;
using DelegateDecompiler;

namespace TheFlow.Site.Models
{
    /// <summary>
    /// Defines a model for an answer to a question that is going to be viewed (not created or edited) by a user.
    /// </summary>
    public class ViewAnswerModel : ViewPostModel
    {

        /// <summary>
        /// Gets or sets wether this answer is accepted.
        /// </summary>
        /// <returns></returns>
        public bool IsAccepted
        {
            get;
            set;
        }

        [Computed]
        public bool IsNotAccepted
        {
            get
            {
                return !IsAccepted;
            }
        }

        public ViewAnswerModel()
        {

        }

        public ViewAnswerModel(Answer answer)
            : base(answer)
        {

        }
    }
}
