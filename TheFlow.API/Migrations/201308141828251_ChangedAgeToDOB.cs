namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAgeToDOB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
            DropColumn("dbo.Users", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Age", c => c.Byte());
            DropColumn("dbo.Users", "DateOfBirth");
        }
    }
}
