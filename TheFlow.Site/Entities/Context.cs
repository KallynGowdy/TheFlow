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
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TheFlow.Api.Entities
{
    /// <summary>
    /// Defines the data context for TheFlow. Use this for all interactions with the database.
    /// </summary>
    public class DbContext : System.Data.Entity.DbContext, TheFlow.Api.Entities.IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Star> Stars { get; set; }
        public DbSet<Edit> Edits { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<UpVote> UpVotes { get; set; }
        public DbSet<DownVote> DownVotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Bind the 'Posts' table to the 'Questions' table and 'Answers' table as a parent
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Answer>().ToTable("Answers");

            modelBuilder.Entity<Vote>().ToTable("Votes");
            modelBuilder.Entity<UpVote>().ToTable("UpVotes");
            modelBuilder.Entity<DownVote>().ToTable("DownVotes");

            modelBuilder.Entity<Post>().Ignore(a => a.Body);
        }
    }
}