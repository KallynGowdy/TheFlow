namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeQuestionTitleRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Title", c => c.String());
        }
    }
}
