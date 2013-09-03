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
using System.Linq;
using System.Text;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for comments to a post.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the ID number of this comment.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        public virtual User Author { get; set; }

        /// <summary>
        /// Gets or sets the date that this comment was posted.
        /// </summary>
        public DateTime DatePosted
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
        /// Gets or sets the post that this comment was added to.
        /// </summary>
        public virtual Post Post { get; set; }

        /// <summary>
        /// Gets or sets the up votes that this comment has.
        /// </summary>
        public uint UpVotes
        {
            get;
            set;
        }
    }
}
