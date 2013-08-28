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

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines a post that contains text information.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets the ID number of this post.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the original body (content) of this post.
        /// </summary>
        [Column(TypeName="ntext")]
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the edits to this post.
        /// </summary>
        public virtual ICollection<Edit> Edits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the author of this post.
        /// </summary>
        [Required]
        public User Author
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
        /// Gets or sets the upvotes that this post has.
        /// </summary>
        public int UpVotes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the down votes this post has.
        /// </summary>
        public int DownVotes
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
        /// Gets the current body of markdown flavored text for this post.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBody()
        {
            if (Edits.Any())
            {
                return Edits.OrderByDescending(a => a.DateChanged.Value).First().Body;
            }
            else
            {
                return Body;
            }
        }
    }
}