﻿// Copyright 2013 Kallyn Gowdy
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheFlow.Api.Models;
using TheFlow.Site;
using TheFlow.Site.Controllers;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for a question.
    /// </summary>
    public class Question : Post
    {
        public Question() { }

        public Question(User author, string body, string title, IEnumerable<Tag> tags)
            : base(author, body)
        {
            this.Title = title;
            this.Tags = tags.ToList();
        }

        public Question(User author, string body, string title, IList<Tag> tags)
            : base(author, body)
        {
            this.Title = title;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets or sets the title of this question.
        /// </summary>
        [Required]
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
        public int Views
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post that is the accepted answer.
        /// </summary>
        [NotMapped]
        public Answer AcceptedAnswer
        {
            get
            {
                return Answers.FirstOrDefault(a => a.Accepted);
            }
            set
            {
                value.ThrowIfNull("value");
                foreach (Answer a in Answers)
                {
                    a.Accepted = false;
                }
                if (Answers.Contains(value))
                {
                    value.Accepted = true;
                }
                else
                {
                    value.Accepted = true;
                    Answers.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of answers to this question.
        /// </summary>
        public virtual ICollection<Answer> Answers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tags that this question references.
        /// </summary>
        public virtual ICollection<Tag> Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the model representation of this entity.
        /// </summary>
        /// <returns></returns>
        public QuestionModel ToModel()
        {
            return new QuestionModel
            {
                Title = this.Title,
                Author = this.Author.DisplayName,
                Body = this.GetMarkdownBody(),
                DateCreated = this.DatePosted
            };
        }

        /// <summary>
        /// Gets the amount of reputation that the given vote is worth.
        /// </summary>
        /// <param name="v">The vote the get the reputation value of.</param>
        /// <returns></returns>
        public override int GetVoteValue(Vote v)
        {
            if (v is DownVote)
            {
                return Settings.Reputation.Questions.DownVote;
            }
            else if (v is UpVote)
            {
                return Settings.Reputation.Questions.UpVote;
            }
            return 0;
        }

        public override System.Linq.Expressions.Expression<Func<int>> GetVoteValueExpression(Vote v)
        {
            if (v is DownVote)
            {
                return () => Settings.Reputation.Questions.DownVote;
            }
            else if (v is UpVote)
            {
                return () => Settings.Reputation.Questions.UpVote;
            }
            return () => 0;
        }

        /// <summary>
        /// Adds the given vote to the post and returns how much reputation that vote is worth, does not add the reputation to the author.
        /// </summary>
        /// <param name="vote"></param>
        /// <returns></returns>
        public override int AddVote(Vote vote)
        {
            if (vote is DownVote)
            {
                this.DownVotes.Add((DownVote)vote);
                return Settings.Reputation.Questions.DownVote;
            }
            else if (vote is UpVote)
            {
                this.UpVotes.Add((UpVote)vote);
                return Settings.Reputation.Questions.UpVote;
            }
            return 0;
        }

        /// <summary>
        /// Removes the given vote from the post and returns how much reputation the removal is worth, does not add/remove that reputation from the author.
        /// </summary>
        /// <param name="vote">The vote to remove from the post.</param>
        /// <returns></returns>
        public override int RemoveVote(Vote vote)
        {
            if (vote is DownVote)
            {
                if (this.DownVotes.Remove((DownVote)vote))
                {
                    return -Settings.Reputation.Questions.DownVote;
                }
            }
            else if (vote is UpVote)
            {
                if (this.UpVotes.Remove((UpVote)vote))
                {
                    return -Settings.Reputation.Questions.UpVote;
                }
            }
            return 0;
        }
    }
}