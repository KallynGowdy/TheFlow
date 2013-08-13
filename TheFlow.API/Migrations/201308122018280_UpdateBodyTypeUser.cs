namespace TheFlow.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBodyTypeUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Body", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Body", c => c.String());
        }
    }
}
