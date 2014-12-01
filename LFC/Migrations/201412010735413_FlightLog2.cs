namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightLog2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FlightLogs", "AirplaneID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.FlightLogs", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.FlightLogs", "AirplaneID");
            CreateIndex("dbo.FlightLogs", "ApplicationUserID");
            AddForeignKey("dbo.FlightLogs", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
            AddForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FlightLogs", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.FlightLogs", new[] { "ApplicationUserID" });
            DropIndex("dbo.FlightLogs", new[] { "AirplaneID" });
            AlterColumn("dbo.FlightLogs", "ApplicationUserID", c => c.String(nullable: false));
            AlterColumn("dbo.FlightLogs", "AirplaneID", c => c.String(nullable: false));
        }
    }
}
