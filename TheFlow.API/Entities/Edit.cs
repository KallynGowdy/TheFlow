using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an edit of text.
    /// </summary>
    public class Edit
    {
        /// <summary>
        /// Gets or sets the Id of the edit.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body text of the edit.
        /// </summary>
        [Required]
        [Column(TypeName="ntext")]
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that edited the text.
        /// </summary>
        [Required]
        public User Editor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the previous version of the text before this edit.
        /// </summary>
        public Edit PreviousVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the original post that was edited.
        /// </summary>
        [Required]
        public Post OriginalPost
        {
            get;
            set;
        }
    }
}