namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentValidations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users");
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Author_OpenId" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            AlterColumn("dbo.Comments", "Body", c => c.String(nullable: false));
            AlterColumn("dbo.Comments", "Author_OpenId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Comments", "Post_Id", c => c.Long(nullable: false));
            AddForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users", "OpenId", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
            CreateIndex("dbo.Comments", "Author_OpenId");
            CreateIndex("dbo.Comments", "Post_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "Author_OpenId" });
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users");
            AlterColumn("dbo.Comments", "Post_Id", c => c.Long());
            AlterColumn("dbo.Comments", "Author_OpenId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "Body", c => c.String());
            CreateIndex("dbo.Comments", "Post_Id");
            CreateIndex("dbo.Comments", "Author_OpenId");
            AddForeignKey("dbo.Comments", "Post_Id", "dbo.Posts", "Id");
            AddForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users", "OpenId");
        }
    }
}
