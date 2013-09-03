namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbstractedVotesToEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateVoted = c.DateTime(nullable: false),
                        Voter_OpenId = c.String(nullable: false, maxLength: 128),
                        Post_Id2 = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Voter_OpenId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Id2, cascadeDelete: true)
                .Index(t => t.Voter_OpenId)
                .Index(t => t.Post_Id2);
            
            CreateTable(
                "dbo.UpVotes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Post_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Votes", t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.DownVotes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Post_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Votes", t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Id)
                .Index(t => t.Post_Id);
            
            DropColumn("dbo.Posts", "UpVotes");
            DropColumn("dbo.Posts", "DownVotes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "DownVotes", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "UpVotes", c => c.Int(nullable: false));
            DropIndex("dbo.DownVotes", new[] { "Post_Id" });
            DropIndex("dbo.DownVotes", new[] { "Id" });
            DropIndex("dbo.UpVotes", new[] { "Post_Id" });
            DropIndex("dbo.UpVotes", new[] { "Id" });
            DropIndex("dbo.Votes", new[] { "Post_Id2" });
            DropIndex("dbo.Votes", new[] { "Voter_OpenId" });
            DropForeignKey("dbo.DownVotes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.DownVotes", "Id", "dbo.Votes");
            DropForeignKey("dbo.UpVotes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.UpVotes", "Id", "dbo.Votes");
            DropForeignKey("dbo.Votes", "Post_Id2", "dbo.Posts");
            DropForeignKey("dbo.Votes", "Voter_OpenId", "dbo.Users");
            DropTable("dbo.DownVotes");
            DropTable("dbo.UpVotes");
            DropTable("dbo.Votes");
        }
    }
}
