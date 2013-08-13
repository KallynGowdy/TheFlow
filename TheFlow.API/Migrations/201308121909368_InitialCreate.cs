namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        OpenId = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false),
                        RealName = c.String(),
                        EmailAddress = c.String(nullable: false),
                        Location = c.String(),
                        Age = c.Byte(),
                    })
                .PrimaryKey(t => t.OpenId);
            
            CreateTable(
                "dbo.Stars",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        User_OpenId = c.String(maxLength: 128),
                        StarredQuestion_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_OpenId)
                .ForeignKey("dbo.Questions", t => t.StarredQuestion_Id)
                .Index(t => t.User_OpenId)
                .Index(t => t.StarredQuestion_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Body = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        UpVotes = c.Int(nullable: false),
                        DownVotes = c.Int(nullable: false),
                        Author_OpenId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_OpenId)
                .Index(t => t.Author_OpenId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DatePosted = c.DateTime(nullable: false),
                        Body = c.String(),
                        Author_OpenId = c.String(maxLength: 128),
                        Post_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_OpenId)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Author_OpenId)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        User_OpenId = c.String(maxLength: 128),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_OpenId)
                .Index(t => t.Id)
                .Index(t => t.User_OpenId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Question_Id = c.Long(nullable: false),
                        User_OpenId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .ForeignKey("dbo.Users", t => t.User_OpenId)
                .Index(t => t.Id)
                .Index(t => t.Question_Id)
                .Index(t => t.User_OpenId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Answers", new[] { "User_OpenId" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "Id" });
            DropIndex("dbo.Questions", new[] { "User_OpenId" });
            DropIndex("dbo.Questions", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "Author_OpenId" });
            DropIndex("dbo.Posts", new[] { "Author_OpenId" });
            DropIndex("dbo.Stars", new[] { "StarredQuestion_Id" });
            DropIndex("dbo.Stars", new[] { "User_OpenId" });
            DropForeignKey("dbo.Answers", "User_OpenId", "dbo.Users");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "Id", "dbo.Posts");
            DropForeignKey("dbo.Questions", "User_OpenId", "dbo.Users");
            DropForeignKey("dbo.Questions", "Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users");
            DropForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users");
            DropForeignKey("dbo.Stars", "StarredQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Stars", "User_OpenId", "dbo.Users");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Stars");
            DropTable("dbo.Users");
        }
    }
}
