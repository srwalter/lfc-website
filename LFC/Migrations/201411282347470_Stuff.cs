namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "MaintenanceOfficer_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Airplanes", "MaintenanceOfficer_Id");
            AddForeignKey("dbo.Airplanes", "MaintenanceOfficer_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Airplanes", "MaintenanceOfficer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Airplanes", new[] { "MaintenanceOfficer_Id" });
            DropColumn("dbo.Airplanes", "MaintenanceOfficer_Id");
        }
    }
}
