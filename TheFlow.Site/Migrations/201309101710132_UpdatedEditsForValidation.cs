namespace TheFlow.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEditsForValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Edits", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Edits", "Body", c => c.String(nullable: false, storeType: "ntext"));
        }
    }
}
