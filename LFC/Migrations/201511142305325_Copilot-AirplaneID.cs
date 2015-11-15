namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopilotAirplaneID : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Copilots", name: "Airplane_AirplaneID", newName: "AirplaneID");
            RenameIndex(table: "dbo.Copilots", name: "IX_Airplane_AirplaneID", newName: "IX_AirplaneID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Copilots", name: "IX_AirplaneID", newName: "IX_Airplane_AirplaneID");
            RenameColumn(table: "dbo.Copilots", name: "AirplaneID", newName: "Airplane_AirplaneID");
        }
    }
}
