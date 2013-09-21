namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPostVotesAround : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Votes", name: "Post_Id", newName: "Post_Id2");
            AddColumn("dbo.UpVotes", "Post_Id", c => c.Long());
            AddColumn("dbo.DownVotes", "Post_Id", c => c.Long());
            AddForeignKey("dbo.UpVotes", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.DownVotes", "Post_Id", "dbo.Posts", "Id");
            CreateIndex("dbo.UpVotes", "Post_Id");
            CreateIndex("dbo.DownVotes", "Post_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DownVotes", new[] { "Post_Id" });
            DropIndex("dbo.UpVotes", new[] { "Post_Id" });
            DropForeignKey("dbo.DownVotes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.UpVotes", "Post_Id", "dbo.Posts");
            DropColumn("dbo.DownVotes", "Post_Id");
            DropColumn("dbo.UpVotes", "Post_Id");
            RenameColumn(table: "dbo.Votes", name: "Post_Id2", newName: "Post_Id");
        }
    }
}
