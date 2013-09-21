namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTagsAndSubscriptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Question_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.TagEdits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        DateChanged = c.DateTime(nullable: false),
                        Editor_OpenId = c.String(nullable: false, maxLength: 128),
                        PreviousVersion_Id = c.Long(),
                        OriginalTag_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Editor_OpenId, cascadeDelete: true)
                .ForeignKey("dbo.TagEdits", t => t.PreviousVersion_Id)
                .ForeignKey("dbo.Tags", t => t.OriginalTag_Id, cascadeDelete: true)
                .Index(t => t.Editor_OpenId)
                .Index(t => t.PreviousVersion_Id)
                .Index(t => t.OriginalTag_Id);
            
            CreateTable(
                "dbo.TagUsers",
                c => new
                    {
                        Tag_Id = c.Long(nullable: false),
                        User_OpenId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.User_OpenId })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_OpenId, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.User_OpenId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TagUsers", new[] { "User_OpenId" });
            DropIndex("dbo.TagUsers", new[] { "Tag_Id" });
            DropIndex("dbo.TagEdits", new[] { "OriginalTag_Id" });
            DropIndex("dbo.TagEdits", new[] { "PreviousVersion_Id" });
            DropIndex("dbo.TagEdits", new[] { "Editor_OpenId" });
            DropIndex("dbo.Tags", new[] { "Question_Id" });
            DropForeignKey("dbo.TagUsers", "User_OpenId", "dbo.Users");
            DropForeignKey("dbo.TagUsers", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagEdits", "OriginalTag_Id", "dbo.Tags");
            DropForeignKey("dbo.TagEdits", "PreviousVersion_Id", "dbo.TagEdits");
            DropForeignKey("dbo.TagEdits", "Editor_OpenId", "dbo.Users");
            DropForeignKey("dbo.Tags", "Question_Id", "dbo.Questions");
            DropTable("dbo.TagUsers");
            DropTable("dbo.TagEdits");
            DropTable("dbo.Tags");
        }
    }
}
