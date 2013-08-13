using System;
namespace TheFlow.API.Entities
{
    public interface IDbContext
    {
        System.Data.Entity.DbSet<Answer> Answers { get; set; }
        System.Data.Entity.DbSet<Comment> Comments { get; set; }
        System.Data.Entity.DbSet<Question> Questions { get; set; }
        System.Data.Entity.DbSet<Star> Stars { get; set; }
        System.Data.Entity.DbSet<User> Users { get; set; }
    }
}
