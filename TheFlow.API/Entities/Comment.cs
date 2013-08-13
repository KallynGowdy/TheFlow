using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for comments to a post.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the ID number of this comment.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        public virtual User Author { get; set; }

        /// <summary>
        /// Gets or sets the date that this comment was posted.
        /// </summary>
        public DateTime DatePosted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the post.
        /// </summary>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post that this comment was added to.
        /// </summary>
        public virtual Post Post { get; set; }

        /// <summary>
        /// Gets or sets the up votes that this comment has.
        /// </summary>
        public uint UpVotes
        {
            get;
            set;
        }
    }
}
