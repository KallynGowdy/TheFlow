using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for a star for a question.
    /// </summary>
    public class Star
    {
        /// <summary>
        /// Gets or sets the id of this star.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that was starred.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the question that is starred.
        /// </summary>
        public virtual Question StarredQuestion
        {
            get;
            set;
        }
    }
}
