namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTagCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "DateCreated");
        }
    }
}
