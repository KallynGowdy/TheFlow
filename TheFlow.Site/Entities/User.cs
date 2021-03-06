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
using System.Linq.Expressions;
using System.Text;
using TheFlow.Api.Models;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines an entity for a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the real name of the user.
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the full name of the user.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the open id that this user owns.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Key]
        public string OpenId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the user.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user (only used for calculating age).
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets the age of the user.
        /// </summary>
        [NotMapped]
        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    return (int)Math.Round((DateTime.UtcNow - DateOfBirth.Value).Days / DateExtensions.YearInDays);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets an expression that gets the age of the user in years.
        /// </summary>
        /// <returns></returns>
        public Expression<Func<int?>> GetAgeExpression()
        {
            if (DateOfBirth.HasValue)
            {
                return () => (int)((DateTime.UtcNow - DateOfBirth.Value).Days / DateExtensions.YearInDays);
            }
            else
            {
                return () => null;
            }
        }

        /// <summary>
        /// Gets or sets the reputation that the user has.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Reputation { get; set; }

        /// <summary>
        /// Gets or sets the date that the user joined 'TheFlow'.
        /// </summary>
        public DateTime DateJoined { get; set; }

        /// <summary>
        /// Gets or sets the preferences of the user.
        /// </summary>
        public Preferences Preferences { get; set; }

        public User()
        {
            DateJoined = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the collection of starred questions that this user has.
        /// </summary>
        public virtual ICollection<Star> StarredQuestions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of answers that this user has posted.
        /// </summary>
        public virtual ICollection<Answer> Answers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of questions that this user has asked.
        /// </summary>
        public virtual ICollection<Question> Questions
        {
            get;
            set;
        }

        public virtual ICollection<Post> Posts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of votes that this user has made.
        /// </summary>
        public virtual ICollection<Vote> Votes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of edits that this user has proposed.
        /// </summary>
        public virtual ICollection<Edit> Edits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of tags that this user is subscribed to.
        /// </summary>
        public virtual ICollection<Tag> Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Gets this user as a model.
        /// </summary>
        /// <returns></returns>
        public UserModel ToModel()
        {
            return new UserModel
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Age = this.Age,
                OpenId = this.OpenId,
                Reputation = this.Reputation,
                Location = this.Location,
                DisplayName = this.DisplayName,
                Preferences = this.Preferences.ToModel()
            };
        }

        /// <summary>
        /// Determines if the given object equals this user.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (obj is User)
                {
                    return ((User)obj).OpenId == this.OpenId;
                }
                else
                {
                    return base.Equals(obj);
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code of this user.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return unchecked(OpenId.GetHashCode() * 21);
        }
    }
}
