using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for a question.
    /// </summary>
    public class Question : Post
    {
        /// <summary>
        /// Gets or sets the title of this question.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the collection of stars of this question.
        /// </summary>
        public virtual ICollection<Star> Stars
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of views that this question has.
        /// </summary>
        public uint Views
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post that is the accepted answer.
        /// </summary>
        public virtual Answer AcceptedAnswer
        {
            get;
            set;
        }
    }
}