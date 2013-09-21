using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.Api.Entities
{
    public class TagEdit
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
        /// Gets or sets the body text of the edit. Maximum lenght is 64,000 characters.
        /// </summary>
        [Required(AllowEmptyStrings=true)]
        [MaxLength(64000)]
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
        public TagEdit PreviousVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the edit was made.
        /// </summary>
        [Required]
        public DateTime? DateChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the original post that was edited.
        /// </summary>
        [Required]
        public Tag OriginalTag
        {
            get;
            set;
        }
    }
}