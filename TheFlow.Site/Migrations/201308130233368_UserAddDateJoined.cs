namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAddDateJoined : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DateJoined", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DateJoined");
        }
    }
}
