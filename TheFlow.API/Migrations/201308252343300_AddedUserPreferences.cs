namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserPreferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Preferences_CodeStyle", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Preferences_CodeStyle");
        }
    }
}
