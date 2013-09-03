namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanedUpAnswerRefsToQuestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "AcceptedAnswer_Id", "dbo.Answers");
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropIndex("dbo.Questions", new[] { "AcceptedAnswer_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            AddColumn("dbo.Answers", "Accepted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Questions", "AcceptedAnswer_Id");
            DropColumn("dbo.Answers", "Question_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "Question_Id1", c => c.Long());
            AddColumn("dbo.Questions", "AcceptedAnswer_Id", c => c.Long());
            DropColumn("dbo.Answers", "Accepted");
            CreateIndex("dbo.Answers", "Question_Id1");
            CreateIndex("dbo.Questions", "AcceptedAnswer_Id");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
            AddForeignKey("dbo.Questions", "AcceptedAnswer_Id", "dbo.Answers", "Id");
        }
    }
}
