namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReputationToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Reputation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Reputation");
        }
    }
}
