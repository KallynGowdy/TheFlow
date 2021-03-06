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
using System.Linq;
using System.Web;

namespace TheFlow.Api.Models
{
    /// <summary>
    /// Defines a model for a user.
    /// Used for network communications(i.e. communications with the API).
    /// All attributes are optional.
    /// </summary>
    [Serializable]
    public class UserModel
    {
        public UserModel() { }

        /// <summary>
        /// Gets or sets the display name of the user model.
        /// </summary>
        [MaxLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the open id of the user model.
        /// </summary>
        public string OpenId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email address of the user model.
        /// </summary>
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user model.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date that the user joined.
        /// </summary>
        public DateTime DateJoined
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the age of the user model.
        /// </summary>
        public int? Age
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of the user model.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user model.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user model.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the reputation of the user model.
        /// This attribute is NOT updated in the database.
        /// </summary>
        public int Reputation { get; set; }

        /// <summary>
        /// Gets or sets the preferences of the user model.
        /// This attribute is updated in the database.
        /// </summary>
        public PreferencesModel Preferences { get; set; }
    }
}