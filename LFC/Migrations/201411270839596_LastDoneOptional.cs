namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastDoneOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AirworthinessDirectives", "LastDoneHours", c => c.Double());
            AlterColumn("dbo.AirworthinessDirectives", "LastDoneDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AirworthinessDirectives", "LastDoneDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AirworthinessDirectives", "LastDoneHours", c => c.Double(nullable: false));
        }
    }
}
