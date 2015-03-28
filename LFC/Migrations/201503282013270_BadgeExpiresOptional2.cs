namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BadgeExpiresOptional2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "BadgeExpires", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "BadgeExpires", c => c.DateTime(nullable: false));
        }
    }
}
