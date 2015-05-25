namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FuelReceipt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FuelReceipts",
                c => new
                    {
                        FuelReceiptID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        AirplaneID = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Gallons = c.Double(nullable: false),
                        Dollars = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.FuelReceiptID)
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FuelReceipts", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.FuelReceipts", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.FuelReceipts", new[] { "AirplaneID" });
            DropIndex("dbo.FuelReceipts", new[] { "ApplicationUserID" });
            DropTable("dbo.FuelReceipts");
        }
    }
}
