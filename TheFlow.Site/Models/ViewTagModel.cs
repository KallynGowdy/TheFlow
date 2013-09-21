using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheFlow.Api.Entities;
using TheFlow.Site.HtmlSanitization;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for a tag that is going to be viewed (not created or edited) by a user.
    /// </summary>
    public class ViewTagModel
    {

        /// <summary>
        /// Creates a new ViewTagModel object given the original tag.
        /// </summary>
        /// <param name="tag"></param>
        public ViewTagModel(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }
            this.Id = tag.Id;
            this.Name = tag.Name;
            this.MarkdownBody = tag.Body;
            this.Subscriptions = tag.Subscribers.Count;
            this.DateCreated = tag.DateCreated;
        }

        /// <summary>
        /// Gets or sets the date that the tag was created.
        /// </summary>
        public DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of subscriptions that this tag has.
        /// </summary>
        public long Subscriptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Id number of the tag.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the direct content from the tag wiki that is not converted or sanitized.
        /// </summary>
        public string MarkdownBody
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the markdown converted body of the tag wiki, not sanitized.
        /// </summary>
        public string ConvertedBody
        {
            get
            {
                return GetConvertedBody(null);
            }
        }

        /// <summary>
        /// Gets the markdown converted body of the tag wiki, sanitized.
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
        /// Gets the markdown converted body of the tag that is sanizited.
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
        /// Gets the markdown converted body of the tag that is not sanitized.
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
    }
}
