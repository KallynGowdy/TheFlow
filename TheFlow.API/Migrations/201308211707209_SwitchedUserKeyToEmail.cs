namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchedUserKeyToEmail : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("dbo.Stars", "User_OpenId", "dbo.Users");
            DropForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users");
            DropForeignKey("dbo.Edits", "Editor_OpenId", "dbo.Users");
            DropForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users");
            DropForeignKey("dbo.Questions", "User_OpenId", "dbo.Users");
            DropForeignKey("dbo.Answers", "User_OpenId", "dbo.Users");
            DropIndex("dbo.Stars", new[] { "User_OpenId" });
            DropIndex("dbo.Posts", new[] { "Author_OpenId" });
            DropIndex("dbo.Edits", new[] { "Editor_OpenId" });
            DropIndex("dbo.Comments", new[] { "Author_OpenId" });
            DropIndex("dbo.Questions", new[] { "User_OpenId" });
            DropIndex("dbo.Answers", new[] { "User_OpenId" });
            RenameColumn(table: "dbo.Stars", name: "User_OpenId", newName: "User_EmailAddress");
            RenameColumn(table: "dbo.Posts", name: "Author_OpenId", newName: "Author_EmailAddress");
            RenameColumn(table: "dbo.Edits", name: "Editor_OpenId", newName: "Editor_EmailAddress");
            RenameColumn(table: "dbo.Comments", name: "Author_OpenId", newName: "Author_EmailAddress");
            RenameColumn(table: "dbo.Questions", name: "User_OpenId", newName: "User_EmailAddress");
            RenameColumn(table: "dbo.Answers", name: "User_OpenId", newName: "User_EmailAddress");
            DropPrimaryKey("dbo.Users", new[] { "OpenId" });
            AlterColumn("dbo.Users", "OpenId", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "EmailAddress", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "EmailAddress");
            AddForeignKey("dbo.Stars", "User_EmailAddress", "dbo.Users", "EmailAddress");
            AddForeignKey("dbo.Posts", "Author_EmailAddress", "dbo.Users", "EmailAddress", cascadeDelete: true);
            AddForeignKey("dbo.Edits", "Editor_EmailAddress", "dbo.Users", "EmailAddress", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "Author_EmailAddress", "dbo.Users", "EmailAddress");
            AddForeignKey("dbo.Questions", "User_EmailAddress", "dbo.Users", "EmailAddress");
            AddForeignKey("dbo.Answers", "User_EmailAddress", "dbo.Users", "EmailAddress");
            CreateIndex("dbo.Stars", "User_EmailAddress");
            CreateIndex("dbo.Posts", "Author_EmailAddress");
            CreateIndex("dbo.Edits", "Editor_EmailAddress");
            CreateIndex("dbo.Comments", "Author_EmailAddress");
            CreateIndex("dbo.Questions", "User_EmailAddress");
            CreateIndex("dbo.Answers", "User_EmailAddress");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Answers", new[] { "User_EmailAddress" });
            DropIndex("dbo.Questions", new[] { "User_EmailAddress" });
            DropIndex("dbo.Comments", new[] { "Author_EmailAddress" });
            DropIndex("dbo.Edits", new[] { "Editor_EmailAddress" });
            DropIndex("dbo.Posts", new[] { "Author_EmailAddress" });
            DropIndex("dbo.Stars", new[] { "User_EmailAddress" });
            DropForeignKey("dbo.Answers", "User_EmailAddress", "dbo.Users");
            DropForeignKey("dbo.Questions", "User_EmailAddress", "dbo.Users");
            DropForeignKey("dbo.Comments", "Author_EmailAddress", "dbo.Users");
            DropForeignKey("dbo.Edits", "Editor_EmailAddress", "dbo.Users");
            DropForeignKey("dbo.Posts", "Author_EmailAddress", "dbo.Users");
            DropForeignKey("dbo.Stars", "User_EmailAddress", "dbo.Users");
            DropPrimaryKey("dbo.Users", new[] { "EmailAddress" });
            AddPrimaryKey("dbo.Users", "OpenId");
            AlterColumn("dbo.Users", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "OpenId", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Answers", name: "User_EmailAddress", newName: "User_OpenId");
            RenameColumn(table: "dbo.Questions", name: "User_EmailAddress", newName: "User_OpenId");
            RenameColumn(table: "dbo.Comments", name: "Author_EmailAddress", newName: "Author_OpenId");
            RenameColumn(table: "dbo.Edits", name: "Editor_EmailAddress", newName: "Editor_OpenId");
            RenameColumn(table: "dbo.Posts", name: "Author_EmailAddress", newName: "Author_OpenId");
            RenameColumn(table: "dbo.Stars", name: "User_EmailAddress", newName: "User_OpenId");
            CreateIndex("dbo.Answers", "User_OpenId");
            CreateIndex("dbo.Questions", "User_OpenId");
            CreateIndex("dbo.Comments", "Author_OpenId");
            CreateIndex("dbo.Edits", "Editor_OpenId");
            CreateIndex("dbo.Posts", "Author_OpenId");
            CreateIndex("dbo.Stars", "User_OpenId");
            AddForeignKey("dbo.Answers", "User_OpenId", "dbo.Users", "OpenId");
            AddForeignKey("dbo.Questions", "User_OpenId", "dbo.Users", "OpenId");
            AddForeignKey("dbo.Comments", "Author_OpenId", "dbo.Users", "OpenId");
            AddForeignKey("dbo.Edits", "Editor_OpenId", "dbo.Users", "OpenId", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "Author_OpenId", "dbo.Users", "OpenId", cascadeDelete: true);
            AddForeignKey("dbo.Stars", "User_OpenId", "dbo.Users", "OpenId");
        }
    }
}
