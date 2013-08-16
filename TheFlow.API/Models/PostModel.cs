using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for a post.
    /// </summary>
    [Serializable]
    public class PostModel
    {
        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        [Required]
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the post was created.
        /// </summary>
        [ReadOnly(true)]
        public DateTime? DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the post which is markdown.
        /// </summary>
        [Required(AllowEmptyStrings=false)]
        public string Body
        {
            get;
            set;
        }
    }
}