using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheFlow.API.Entities;

namespace TheFlow.API.Models
{
    /// <summary>
    /// Defines a model for a post that is going to be viewed by the user.
    /// </summary>
    public class ViewPostModel
    {
        public ViewPostModel()
        {
        }

        public ViewPostModel(Post post)
        {
            post.ThrowIfNull();
            this.Author = new UserModel
            {
                Age = post.Author.Age,
                DisplayName = post.Author.DisplayName,
                Location = post.Author.Location,
                Reputation = post.Author.Reputation,
                Preferences = post.Author.Preferences == null ? (post.Author.Preferences = new Preferences()).ToModel() : post.Author.Preferences.ToModel()
            };
            this.Id = post.Id;
            this.Body = post.Body;
            this.DateCreated = post.DatePosted.Value;
            this.DownVotes = post.DownVotes;
            this.UpVotes = post.UpVotes;
        }

        /// <summary>
        /// Gets or set the ID number of the post.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        public UserModel Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the post was posted.
        /// </summary>
        public DateTime DateCreated
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
        /// Gets or sets the upvotes on this post.
        /// </summary>
        public int UpVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the downvotes on this post.
        /// </summary>
        public int DownVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the net vote on this post.
        /// </summary>
        public int NetVote
        {
            get
            {
                return UpVotes - DownVotes;
            }
        }
    }
}