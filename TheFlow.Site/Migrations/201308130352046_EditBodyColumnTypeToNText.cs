namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBodyColumnTypeToNText : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Edits", "Body", c => c.String(nullable: false, storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Edits", "Body", c => c.String(nullable: false));
        }
    }
}
