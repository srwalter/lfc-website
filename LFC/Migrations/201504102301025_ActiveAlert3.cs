namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveAlert3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirworthinessDirective_KeyNum", "dbo.AirworthinessDirectives");
            DropIndex("dbo.ActiveAlerts", new[] { "AirworthinessDirective_KeyNum" });
            DropPrimaryKey("dbo.ActiveAlerts");
            AddPrimaryKey("dbo.ActiveAlerts", new[] { "Type", "AirplaneID" });
            DropColumn("dbo.ActiveAlerts", "AirworthinessDirectiveID");
            //DropColumn("dbo.ActiveAlerts", "AirworthinessDirective_KeyNum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActiveAlerts", "AirworthinessDirective_KeyNum", c => c.Int());
            AddColumn("dbo.ActiveAlerts", "AirworthinessDirectiveID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.ActiveAlerts");
            AddPrimaryKey("dbo.ActiveAlerts", new[] { "AirplaneID", "Type", "AirworthinessDirectiveID" });
            CreateIndex("dbo.ActiveAlerts", "AirworthinessDirective_KeyNum");
            AddForeignKey("dbo.ActiveAlerts", "AirworthinessDirective_KeyNum", "dbo.AirworthinessDirectives", "KeyNum");
        }
    }
}
