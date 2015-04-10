namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllDuckedUp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.ActiveAlerts", new[] { "AirplaneID" });
            DropTable("dbo.ActiveAlerts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ActiveAlerts",
                c => new
                    {
                        Type = c.Int(nullable: false),
                        AirplaneID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Type, t.AirplaneID });
            
            CreateIndex("dbo.ActiveAlerts", "AirplaneID");
            AddForeignKey("dbo.ActiveAlerts", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
        }
    }
}
