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

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for down votes.
    /// </summary>
    public class DownVote : Vote
    {
        /// <summary>
        /// Gets the string representation of the down vote.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} Down Voted the post by {1}", Voter.DisplayName, Post.Author.DisplayName);
        }
    }
}
