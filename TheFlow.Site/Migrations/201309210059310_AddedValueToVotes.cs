namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedValueToVotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "Value");
        }
    }
}
