namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnswersReferenceToQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "AcceptedAnswer_Id", c => c.Long());
            AddColumn("dbo.Answers", "Question_Id1", c => c.Long());
            AddForeignKey("dbo.Questions", "AcceptedAnswer_Id", "dbo.Answers", "Id");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
            CreateIndex("dbo.Questions", "AcceptedAnswer_Id");
            CreateIndex("dbo.Answers", "Question_Id1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            DropIndex("dbo.Questions", new[] { "AcceptedAnswer_Id" });
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropForeignKey("dbo.Questions", "AcceptedAnswer_Id", "dbo.Answers");
            DropColumn("dbo.Answers", "Question_Id1");
            DropColumn("dbo.Questions", "AcceptedAnswer_Id");
        }
    }
}
