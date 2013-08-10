using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.API.Models
{
    [Serializable]
    public class QuestionModel : PostModel
    {
        /// <summary>
        /// Gets or sets the tags used to mark the question.
        /// </summary>
        [Required]
        public IEnumerable<string> Tags
        {
            get;
            set;
        }
    }
}