namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirplaneCheckout : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirplaneCheckouts",
                c => new
                    {
                        PilotID = c.String(nullable: false, maxLength: 128),
                        AirplaneID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PilotID, t.AirplaneID })
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.PilotID, cascadeDelete: true)
                .Index(t => t.PilotID)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirplaneCheckouts", "PilotID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AirplaneCheckouts", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirplaneCheckouts", new[] { "AirplaneID" });
            DropIndex("dbo.AirplaneCheckouts", new[] { "PilotID" });
            DropTable("dbo.AirplaneCheckouts");
        }
    }
}
