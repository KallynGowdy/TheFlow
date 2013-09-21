using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheFlow.Site.Controllers;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for a tag.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the Id number of this tag.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the users subscribed to this tag.
        /// </summary>
        public virtual ICollection<User> Subscribers
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new empty tag.
        /// </summary>
        public Tag() { }

        /// <summary>
        /// Creates a new tag given the name, body of the wiki page and creator.
        /// </summary>
        /// <param name="name">The name of the tag to create.</param>
        /// <param name="wikiBody">The markdown body of the wiki page to create for the tag.</param>
        /// <param name="creator">The user who is the creator of the tag.</param>
        public Tag(string name, string wikiBody, User creator)
        {
            this.Name = name;
            if (wikiBody == null)
            {
                this.SetBody(string.Empty, creator);
            }
            else
            {
                this.SetBody(wikiBody, creator);
            }
            this.DateCreated = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the creator of this tag.
        /// </summary>
        [NotMapped]
        public User Creator
        {
            get
            {
                return Edits.OrderBy(a => a.DateChanged).First().Editor;
            }
        }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        [Required]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the date that the tag was created.
        /// </summary>
        public DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current body (content) that is not converted or sanitized from this tag.
        /// Use SetBody to set the body content of the tag.
        /// </summary>
        public string Body
        {
            get
            {
                return GetCurrentBody();
            }
        }

        /// <summary>
        /// Gets the current body of markdown flavored text for this tag.
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

        /// <summary>
        /// Gets the plain-text version of the body of the tag wiki that is stripped from all markdown.
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
        /// Gets the sanitized markdown-converted version of the body of this tag.
        /// </summary>
        /// <returns></returns>
        public string GetMarkdownBody()
        {
            Markdown m = new Markdown(true);
            return ControllerHelper.HtmlSanitizer.GetHtml(m.Transform(GetCurrentBody()));
        }

        /// <summary>
        /// Gets the original body (content) of this tag.
        /// </summary>
        [NotMapped]
        public string OriginalBody
        {
            get
            {
                return Edits.OrderBy(e => e.DateChanged).First().Body;
            }
        }

        private ICollection<TagEdit> edits;

        /// <summary>
        /// Gets or sets the different revisions of the Content for the tag wiki.
        /// </summary>
        public virtual ICollection<TagEdit> Edits
        {
            get
            {
                return edits ?? (edits = new Collection<TagEdit>());
            }
            protected set
            {
                edits = value;
            }
        }

        /// <summary>
        /// Sets the content of the body by creating a new edit by the given user.
        /// </summary>
        public void SetBody(string newBody, User editor)
        {
            if (newBody != null && editor != null)
            {
                Edits.Add(new TagEdit
                {
                    Body = newBody,
                    DateChanged = DateTime.UtcNow,
                    Editor = editor,
                    OriginalTag = this,
                    PreviousVersion = Edits.OrderByDescending(a => a.DateChanged).FirstOrDefault()
                });
            }
        }

        /// <summary>
        /// Gets the name of this tag.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}