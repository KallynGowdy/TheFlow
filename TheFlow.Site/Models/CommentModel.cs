using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.Site.Models
{
    /// <summary>
    /// Defines a model for a comment that is posted to the server.
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// Gets or Sets the body of the comment. Minimum length allowed is 10. Required.
        /// </summary>
        [Required]
        [MinLength(10)]
        public string Body
        {
            get;
            set;
        }

    }
}