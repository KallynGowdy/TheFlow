// Copyright 2013 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TheFlow.Api.Authentication;
using TheFlow.Site.Controllers;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines a post that contains text information.
    /// </summary>
    public abstract class Post
    {
        protected Post() { }

        /// <summary>
        /// Creates a new post that was posted at DateTime.UtcNow, posted by the given author with the given body.
        /// </summary>
        /// <param name="author"></param>
        /// <param name="body"></param>
        protected Post(User author, string body)
        {
            this.Author = author;
            this.DatePosted = DateTime.UtcNow;
            this.Edits.Add(new Edit
            {
                Editor = author,
                Body = body,
                DateChanged = DateTime.UtcNow,
                OriginalPost = this,
                PreviousVersion = null
            });
        }

        /// <summary>
        /// Gets or sets the ID number of this post.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets the current body (content) of this post.
        /// User SetBody to set the body content of the post.
        /// </summary>
        public string Body
        {
            get
            {
                return GetCurrentBody();
            }
        }

        /// <summary>
        /// Gets the plain-text version of the body of the post that is stripped from all markdown.
        /// </summary>
        public string PlainTextBody
        {
            get
            {
                string m = GetMarkdownBody();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(m);
                return doc.DocumentNode.InnerText;
            }
        }

        /// <summary>
        /// Gets the sanitized markdown-converted version of the body of this post.
        /// </summary>
        /// <returns></returns>
        public string GetMarkdownBody()
        {
            Markdown m = new Markdown(true);
            return ControllerHelper.HtmlSanitizer.GetHtml(m.Transform(GetCurrentBody()));
        }

        /// <summary>
        /// Sets the content of the body by creating a new edit by the given user.
        /// </summary>
        /// <param name="?"></param>
        public void SetBody(string newBody, User editor)
        {
            if (newBody != null && editor != null)
            {
                Edits.Add(new Edit
                {
                    Body = newBody,
                    DateChanged = DateTime.UtcNow,
                    Editor = editor,
                    OriginalPost = this,
                    PreviousVersion = Edits.OrderByDescending(a => a.DateChanged).FirstOrDefault()
                });
            }
        }

        /// <summary>
        /// Gets the original body (content) of this post.
        /// </summary>
        [NotMapped]
        public string OriginalBody
        {
            get
            {
                return Edits.OrderBy(e => e.DateChanged).First().Body;
            }
        }

        private ICollection<Edit> edits;

        /// <summary>
        /// Gets or protected sets the edits to this post.
        /// </summary>
        public virtual ICollection<Edit> Edits
        {
            get
            {
                return edits ?? (edits = new Collection<Edit>());
            }
            protected set
            {
                edits = value;
            }
        }

        /// <summary>
        /// Gets or sets the author of this post.
        /// </summary>
        [Required]
        public virtual User Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that this post was posted.
        /// </summary>
        [Required]
        public DateTime? DatePosted { get; set; }

        /// <summary>
        /// Gets the net number of votes on the post.
        /// </summary>
        /// <returns></returns>
        public int NetVotes()
        {
            return UpVotes.Count() - DownVotes.Count();
        }

        ///// <summary>
        ///// Gets the upvotes that this post has.
        ///// </summary>
        //public virtual IEnumerable<UpVote> UpVotes
        //{
        //    get
        //    {
        //        return Votes.OfType<UpVote>();
        //    }
        //}

        ///// <summary>
        ///// Gets the down votes this post has.
        ///// </summary>
        //public virtual IEnumerable<DownVote> DownVotes
        //{
        //    get
        //    {
        //        return Votes.OfType<DownVote>();
        //    }
        //}

        /// <summary>
        /// Gets or sets the collection of up votes on this post.
        /// Use AddVote and RemoveVote to integrate reputation changes.
        /// </summary>
        public virtual ICollection<UpVote> UpVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of down votes on this post.
        /// Use AddVote and RemoveVote to integrate reputation changes.
        /// </summary>
        public virtual ICollection<DownVote> DownVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the votes on this post.
        /// </summary>
        public virtual ICollection<Vote> Votes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of comments that refer to this post.
        /// </summary>
        public virtual ICollection<Comment> Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Adds the given vote to the post and returns how much reputation it is worth, does not add that reputation to the author.
        /// </summary>
        /// <param name="vote">The vote to add to the post.</param>
        /// <returns></returns>
        public abstract int AddVote(Vote vote);

        /// <summary>
        /// Removes the given vote from the post and returns how much reputation the removal is worth, does not add/remove that reputation from the author.
        /// </summary>
        /// <param name="vote">The vote to remove from the post.</param>
        /// <returns></returns>
        public abstract int RemoveVote(Vote vote);

        /// <summary>
        /// Gets the amount of reputation that the given vote is worth.
        /// </summary>
        /// <param name="v">The vote the get the reputation value of.</param>
        /// <returns></returns>
        public abstract int GetVoteValue(Vote v);

        public abstract Expression<Func<int>> GetVoteValueExpression(Vote v);

        /// <summary>
        /// Gets the current body of markdown flavored text for this post.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBody()
        {
            if (Edits.Count > 0)
            {
                return Edits.OrderByDescending(a => a.DateChanged.Value).First().Body;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}