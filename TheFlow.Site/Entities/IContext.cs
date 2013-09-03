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
namespace TheFlow.Api.Entities
{
    public interface IDbContext
    {
        System.Data.Entity.DbSet<Answer> Answers { get; set; }
        System.Data.Entity.DbSet<Comment> Comments { get; set; }
        System.Data.Entity.DbSet<Question> Questions { get; set; }
        System.Data.Entity.DbSet<Star> Stars { get; set; }
        System.Data.Entity.DbSet<User> Users { get; set; }
        System.Data.Entity.DbSet<Post> Posts { get; set; }
        System.Data.Entity.DbSet<Vote> Votes { get; set; }
    }
}