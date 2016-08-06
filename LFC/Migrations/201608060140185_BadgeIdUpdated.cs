namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BadgeIdUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BadgeIdUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BadgeIdUpdated");
        }
    }
}
