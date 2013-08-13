namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEdits : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "Author_OpenId" });
            CreateTable(
                "dbo.Edits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Body = c.String(nullable: false),
                        Editor_OpenId = c.String(nullable: false, maxLength: 128),
                        PreviousVersion_Id = c.Long(),
                        OriginalPost_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Editor_OpenId, cascadeDelete: true)
                .ForeignKey("dbo.Edits", t => t.PreviousVersion_Id)
                .ForeignKey("dbo.Posts", t => t.OriginalPost_Id, cascadeDelete: true)
                .Index(t => t.Editor_OpenId)
                .Index(t => t.PreviousVersion_Id)
                .Index(t => t.OriginalPost_Id);
            
            AlterColumn("dbo.Posts", "Body", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.Posts", "Author_OpenId", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users", "OpenId", cascadeDelete: false);
            CreateIndex("dbo.Posts", "Author_OpenId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Edits", new[] { "OriginalPost_Id" });
            DropIndex("dbo.Edits", new[] { "PreviousVersion_Id" });
            DropIndex("dbo.Edits", new[] { "Editor_OpenId" });
            DropIndex("dbo.Posts", new[] { "Author_OpenId" });
            DropForeignKey("dbo.Edits", "OriginalPost_Id", "dbo.Posts");
            DropForeignKey("dbo.Edits", "PreviousVersion_Id", "dbo.Edits");
            DropForeignKey("dbo.Edits", "Editor_OpenId", "dbo.Users");
            DropForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users");
            AlterColumn("dbo.Posts", "Author_OpenId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "Body", c => c.String(storeType: "ntext"));
            DropTable("dbo.Edits");
            CreateIndex("dbo.Posts", "Author_OpenId");
            AddForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users", "OpenId");
        }
    }
}
