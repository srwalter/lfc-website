namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BadgeExpires : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BadgeExpires", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BadgeExpires");
        }
    }
}
