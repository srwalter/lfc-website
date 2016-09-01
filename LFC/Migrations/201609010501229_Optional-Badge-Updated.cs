namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalBadgeUpdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "BadgeIdUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "BadgeIdUpdated", c => c.DateTime(nullable: false));
        }
    }
}
