namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSecondRefFromPostToVote : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UpVotes", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.DownVotes", "Post_Id", "dbo.Posts");
            DropIndex("dbo.UpVotes", new[] { "Post_Id" });
            DropIndex("dbo.DownVotes", new[] { "Post_Id" });
            RenameColumn(table: "dbo.Votes", name: "Post_Id2", newName: "Post_Id");
            DropColumn("dbo.UpVotes", "Post_Id");
            DropColumn("dbo.DownVotes", "Post_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DownVotes", "Post_Id", c => c.Long());
            AddColumn("dbo.UpVotes", "Post_Id", c => c.Long());
            RenameColumn(table: "dbo.Votes", name: "Post_Id", newName: "Post_Id2");
            CreateIndex("dbo.DownVotes", "Post_Id");
            CreateIndex("dbo.UpVotes", "Post_Id");
            AddForeignKey("dbo.DownVotes", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.UpVotes", "Post_Id", "dbo.Posts", "Id");
        }
    }
}
