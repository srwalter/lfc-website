namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBadgeID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BadgeID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BadgeID");
        }
    }
}
