using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.Site.Models
{
    /// <summary>
    /// Defines a model for answers that are created by the user.
    /// </summary>
    public class AnswerModel : PostModel
    {

        /// <summary>
        /// Gets or sets the Id number of the question to post this as an answer to.
        /// </summary>
        [Required]
        public long? QuestionId
        {
            get;
            set;
        }
    }
}