namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopilotAirplaneRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Copilots", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.Copilots", new[] { "AirplaneID" });
            AlterColumn("dbo.Copilots", "AirplaneID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Copilots", "AirplaneID");
            AddForeignKey("dbo.Copilots", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Copilots", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.Copilots", new[] { "AirplaneID" });
            AlterColumn("dbo.Copilots", "AirplaneID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Copilots", "AirplaneID");
            AddForeignKey("dbo.Copilots", "AirplaneID", "dbo.Airplanes", "AirplaneID");
        }
    }
}
