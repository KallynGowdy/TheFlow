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

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an edit of text.
    /// </summary>
    public class Edit
    {
        /// <summary>
        /// Gets or sets the Id of the edit.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body text of the edit.
        /// </summary>
        [Required]
        [Column(TypeName="ntext")]
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that edited the text.
        /// </summary>
        [Required]
        public User Editor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the previous version of the text before this edit.
        /// </summary>
        public Edit PreviousVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date that the edit was made.
        /// </summary>
        [Required]
        public DateTime? DateChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the original post that was edited.
        /// </summary>
        [Required]
        public Post OriginalPost
        {
            get;
            set;
        }
    }
}