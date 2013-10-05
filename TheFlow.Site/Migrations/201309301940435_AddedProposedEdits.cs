namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProposedEdits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Edits", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Edits", "Accepted");
        }
    }
}
