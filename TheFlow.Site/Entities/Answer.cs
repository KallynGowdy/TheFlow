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
using System.Text;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for an answer.
    /// </summary>
    public class Answer : Post
    {
        public Answer() { }

        public Answer(User author, string body, Question question, bool accepted = false)
            : base(author, body)
        {
            this.Question = question;
            this.Accepted = accepted;
        }

        /// <summary>
        /// Gets or sets the question that this post is an answer to.
        /// </summary>
        [Required]
        public Question Question
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the answer is accepted.
        /// </summary>
        public bool Accepted
        {
            get;
            set;
        }
    }
}