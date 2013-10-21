using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFlow.Site.Models
{
    /// <summary>
    /// Defines a class that is used to hold proposed changes to a post.
    /// Usually posted back from the client.
    /// </summary>
    public class EditPostModel
    {
        /// <summary>
        /// Gets or sets the Id of the post to edit.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the post. (Only used for questions).
        /// </summary>
        public string Title
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
    }
}