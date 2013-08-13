using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for an answer.
    /// </summary>
    public class Answer : Post
    {
        /// <summary>
        /// Gets or sets the question that this post is an answer to.
        /// </summary>
        [Required]
        public Question Question
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if this answer is the accepted answer.
        /// </summary>
        /// <returns></returns>
        public bool IsAccepted()
        {
            return Question.AcceptedAnswer == this;
        }
    }
}
