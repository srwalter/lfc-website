namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopilotAddAirplane : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Copilots", "Airplane_AirplaneID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Copilots", "Airplane_AirplaneID");
            AddForeignKey("dbo.Copilots", "Airplane_AirplaneID", "dbo.Airplanes", "AirplaneID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Copilots", "Airplane_AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.Copilots", new[] { "Airplane_AirplaneID" });
            DropColumn("dbo.Copilots", "Airplane_AirplaneID");
        }
    }
}
