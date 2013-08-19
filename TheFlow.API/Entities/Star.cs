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
using System.Linq;
using System.Text;

namespace TheFlow.API.Entities
{
    /// <summary>
    /// Defines an entity for a star for a question.
    /// </summary>
    public class Star
    {
        /// <summary>
        /// Gets or sets the id of this star.
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user that was starred.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the question that is starred.
        /// </summary>
        public virtual Question StarredQuestion
        {
            get;
            set;
        }
    }
}
