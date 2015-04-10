namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveAlertDoOver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveAlerts",
                c => new
                    {
                        Type = c.Int(nullable: false),
                        AirplaneID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Type, t.AirplaneID })
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID, cascadeDelete: true)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.ActiveAlerts", new[] { "AirplaneID" });
            DropTable("dbo.ActiveAlerts");
        }
    }
}
