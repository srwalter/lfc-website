namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillingToPlane : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.FlightLogs", new[] { "ApplicationUserID" });
            AlterColumn("dbo.FlightLogs", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.FlightLogs", "ApplicationUserID");
            AddForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.FlightLogs", new[] { "ApplicationUserID" });
            AlterColumn("dbo.FlightLogs", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.FlightLogs", "ApplicationUserID");
            AddForeignKey("dbo.FlightLogs", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
