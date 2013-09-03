namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFirstLastName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String());
            AddColumn("dbo.Users", "LastName", c => c.String());
            DropColumn("dbo.Users", "RealName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "RealName", c => c.String());
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "FirstName");
        }
    }
}
