namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UpVotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "UpVotes");
        }
    }
}
