using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines a subscription from a user to a tag.
    /// </summary>
    public class TagSubscription
    {
        /// <summary>
        /// Gets or sets the id of the subscription.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subscriber to the tag.
        /// </summary>
        public User Subscriber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tag that the user is subscribing to.
        /// </summary>
        public Tag Tag
        {
            get;
            set;
        }
    }
}