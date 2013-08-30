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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TheFlow.API.Models;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for a question.
    /// </summary>
    public class Question : Post
    {
        public Question() { }

        public Question(User author, string body, string title)
            : base(author, body)
        {
            this.Title = title;
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
        public uint Views
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
        /// Gets the model representation of this entity.
        /// </summary>
        /// <returns></returns>
        public QuestionModel ToModel()
        {
            return new QuestionModel
            {
                Title = this.Title,
                Author = this.Author.DisplayName,
                Body = this.Body,
                DateCreated = this.DatePosted
            };
        }
    }
}