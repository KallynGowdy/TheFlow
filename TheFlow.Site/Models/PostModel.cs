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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for a post.
    /// </summary>
    [Serializable]
    public class PostModel
    {
        /// <summary>
        /// Gets or sets the author of the post.
        /// </summary>
        [ReadOnly(true)]
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the post was created.
        /// </summary>
        [ReadOnly(true)]
        public DateTime? DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body of the post which is markdown.
        /// </summary>
        [Required(AllowEmptyStrings=false)]
        [MinLength(40)]
        public string Body
        {
            get;
            set;
        }

    }
}