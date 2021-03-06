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
using TheFlow.Site;

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
        public virtual Question Question
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

        /// <summary>
        /// Gets the amount of reputation that the given vote is worth.
        /// </summary>
        /// <param name="v">The vote the get the reputation value of.</param>
        /// <returns></returns>
        public override int GetVoteValue(Vote v)
        {
            if (v is DownVote)
            {
                return Settings.Reputation.Answers.DownVote;
            }
            else if (v is UpVote)
            {
                return Settings.Reputation.Answers.UpVote;
            }
            return 0;
        }

        public override System.Linq.Expressions.Expression<Func<int>> GetVoteValueExpression(Vote v)
        {
            if (v is DownVote)
            {
                return () => Settings.Reputation.Answers.DownVote;
            }
            else if (v is UpVote)
            {
                return () => Settings.Reputation.Answers.UpVote;
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
                return Settings.Reputation.Answers.DownVote;
            }
            else if (vote is UpVote)
            {
                this.UpVotes.Add((UpVote)vote);
                return Settings.Reputation.Answers.UpVote;
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
                    return -Settings.Reputation.Answers.DownVote;
                }
            }
            else if (vote is UpVote)
            {
                if (this.UpVotes.Remove((UpVote)vote))
                {
                    return -Settings.Reputation.Answers.UpVote;
                }
            }
            return 0;
        }
    }
}
