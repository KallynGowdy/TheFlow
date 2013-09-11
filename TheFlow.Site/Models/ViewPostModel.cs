using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TheFlow.Api.Entities;
using TheFlow.Site;
using TheFlow.Site.HtmlSanitization;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for a post that is going to be viewed by the user.
    /// </summary>
    public abstract class ViewPostModel
    {
        protected ViewPostModel()
        {
        }

        protected ViewPostModel(Post post)
        {
            post.ThrowIfNull();
            this.Author = new UserModel
            {
                Age = post.Author.Age,
                DisplayName = post.Author.DisplayName,
                OpenId = post.Author.OpenId,
                Location = post.Author.Location,
                Reputation = post.Author.Reputation,
                Preferences = post.Author.Preferences == null ? (post.Author.Preferences = new Preferences()).ToModel() : post.Author.Preferences.ToModel()
            };
            this.Id = post.Id;
            this.MarkdownBody = post.Body;
            this.DateCreated = post.DatePosted.Value;
            this.DownVotes = post.DownVotes.Select(v => new ViewDownVoteModel(v));
            this.UpVotes = post.UpVotes.Select(v => new ViewUpVoteModel(v));
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
        /// Gets or sets the direct content from the post that is not converted or sanitized.
        /// </summary>
        public string MarkdownBody
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the markdown converted body of the post, not sanitized.
        /// </summary>
        public string ConvertedBody
        {
            get
            {
                return GetConvertedBody(null);
            }
        }

        /// <summary>
        /// Gets the markdown converted body of the post, sanitized.
        /// </summary>
        public string SanitizedBody
        {
            get
            {
                return GetSanitizedBody(null, TheFlow.Site.Controllers.ControllerHelper.HtmlSanitizer);
            }
        }

        /// <summary>
        /// Gets the text-only version of the body stripped of all markdown.
        /// </summary>
        public string PlainTextBody
        {
            get
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(SanitizedBody);
                return doc.DocumentNode.InnerText;
            }
        }

        /// <summary>
        /// Gets the markdown converted body of the post that is sanizited.
        /// </summary>
        /// <param name="transformer">The Markdown object used to convert markdown to html. Optional.</param>
        /// <param name="sanitizer">The IHtmlSanitizer object used to sanitize the html produced by the converter.</param>
        /// <returns>A string containing the Html that was produced and then sanitized from the markdown.</returns>
        public string GetSanitizedBody(MarkdownSharp.Markdown transformer = null, IHtmlSanitizer sanitizer = null)
        {
            if (sanitizer == null)
            {
                return new HtmlSanitizer().GetHtml(GetConvertedBody(transformer));
            }
            else
            {
                return sanitizer.GetHtml(GetConvertedBody(transformer));
            }
        }

        /// <summary>
        /// Gets the markdown converted body of the post that is not sanitized.
        /// </summary>
        /// <param name="transformer">The Markdown object used to convert markdown to html. Optional.</param>
        /// <returns>A string containing the Html that was produced from the markdown.</returns>
        public string GetConvertedBody(MarkdownSharp.Markdown transformer = null)
        {
            if (transformer == null)
            {
                return (new MarkdownSharp.Markdown(true)).Transform(MarkdownBody);
            }
            else
            {
                return transformer.Transform(MarkdownBody);
            }
        }

        /// <summary>
        /// Gets or sets the upvotes on this post.
        /// </summary>
        public IEnumerable<ViewUpVoteModel> UpVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the downvotes on this post.
        /// </summary>
        public IEnumerable<ViewDownVoteModel> DownVotes
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
                return UpVotes.Count() - DownVotes.Count();
            }
        }

        /// <summary>
        /// Gets the total number of votes on this post.
        /// </summary>
        public long TotalVotes
        {
            get
            {
                return ((long)UpVotes.Count() + (long)DownVotes.Count());
            }
        }
    }
}