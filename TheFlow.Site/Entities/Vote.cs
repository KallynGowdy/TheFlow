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
using System.Linq;
using System.Web;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for a vote.
    /// </summary>
    public abstract class Vote
    {

        /// <summary>
        /// Gets or set the user that voted.
        /// </summary>
        [Required]
        public virtual User Voter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the user voted. required.
        /// </summary>
        [Required]
        public DateTime? DateVoted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post that was voted on.
        /// </summary>
        [Required]
        public virtual Post Post
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reputation value of the vote.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Id of the vote.
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }
    }
}