using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFlow.Api.Entities;
using TheFlow.Api.Models;

namespace TheFlow.Site.Models
{
    /// <summary>
    /// Defines a model for a comment that is going to be viewed by the user.
    /// </summary>
    public class ViewCommentModel
    {
        public ViewCommentModel() { }

        public ViewCommentModel(Comment comment)
        {
            this.Body = comment.Body;
            this.Author = comment.Author.ToModel();
            this.DatePosted = comment.DatePosted;
            this.UpVotes = comment.UpVotes;
            this.Id = comment.Id;
        }

        /// <summary>
        /// Gets or sets the id number of the comment.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author of the comment.
        /// </summary>
        public UserModel Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the comment that is markdown.
        /// </summary>
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the markdown-converted and then sanitized body of the content of this comment.
        /// </summary>
        public string SanitizedBody
        {
            get
            {
                return Controllers.ControllerHelper.HtmlSanitizer.GetHtml(new MarkdownSharp.Markdown(true).Transform(Body));
            }
        }

        /// <summary>
        /// Gets or sets the date that the comment was posted.
        /// </summary>
        public DateTime DatePosted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of up votes that this comment has.
        /// </summary>
        public int UpVotes
        {
            get;
            set;
        }
    }
}
