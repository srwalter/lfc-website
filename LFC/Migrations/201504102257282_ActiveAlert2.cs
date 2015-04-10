namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveAlert2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirplaneID_AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.ActiveAlerts", new[] { "AirplaneID_AirplaneID" });
            RenameColumn(table: "dbo.ActiveAlerts", name: "AirplaneID_AirplaneID", newName: "AirplaneID");
            RenameColumn(table: "dbo.ActiveAlerts", name: "AirworthinessDirectiveID_KeyNum", newName: "AirworthinessDirective_KeyNum");
            RenameIndex(table: "dbo.ActiveAlerts", name: "IX_AirworthinessDirectiveID_KeyNum", newName: "IX_AirworthinessDirective_KeyNum");
            DropPrimaryKey("dbo.ActiveAlerts");
            AddColumn("dbo.ActiveAlerts", "AirworthinessDirectiveID", c => c.Int(nullable: false));
            AlterColumn("dbo.ActiveAlerts", "AirplaneID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ActiveAlerts", new[] { "AirplaneID", "Type", "AirworthinessDirectiveID" });
            CreateIndex("dbo.ActiveAlerts", "AirplaneID");
            AddForeignKey("dbo.ActiveAlerts", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.ActiveAlerts", new[] { "AirplaneID" });
            DropPrimaryKey("dbo.ActiveAlerts");
            AlterColumn("dbo.ActiveAlerts", "AirplaneID", c => c.String(maxLength: 128));
            DropColumn("dbo.ActiveAlerts", "AirworthinessDirectiveID");
            AddPrimaryKey("dbo.ActiveAlerts", "Type");
            RenameIndex(table: "dbo.ActiveAlerts", name: "IX_AirworthinessDirective_KeyNum", newName: "IX_AirworthinessDirectiveID_KeyNum");
            RenameColumn(table: "dbo.ActiveAlerts", name: "AirworthinessDirective_KeyNum", newName: "AirworthinessDirectiveID_KeyNum");
            RenameColumn(table: "dbo.ActiveAlerts", name: "AirplaneID", newName: "AirplaneID_AirplaneID");
            CreateIndex("dbo.ActiveAlerts", "AirplaneID_AirplaneID");
            AddForeignKey("dbo.ActiveAlerts", "AirplaneID_AirplaneID", "dbo.Airplanes", "AirplaneID");
        }
    }
}
