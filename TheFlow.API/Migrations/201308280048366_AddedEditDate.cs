namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEditDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Edits", "DateChanged", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Edits", "DateChanged");
        }
    }
}
