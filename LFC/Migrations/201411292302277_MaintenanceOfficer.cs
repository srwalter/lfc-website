namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaintenanceOfficer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Airplanes", name: "MaintenanceOfficer_Id", newName: "MaintenanceOfficerID");
            RenameIndex(table: "dbo.Airplanes", name: "IX_MaintenanceOfficer_Id", newName: "IX_MaintenanceOfficerID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Airplanes", name: "IX_MaintenanceOfficerID", newName: "IX_MaintenanceOfficer_Id");
            RenameColumn(table: "dbo.Airplanes", name: "MaintenanceOfficerID", newName: "MaintenanceOfficer_Id");
        }
    }
}
