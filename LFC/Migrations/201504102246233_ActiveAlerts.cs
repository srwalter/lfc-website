namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveAlerts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveAlerts",
                c => new
                    {
                        Type = c.Int(nullable: false),
                        AirplaneID_AirplaneID = c.String(maxLength: 128),
                        AirworthinessDirectiveID_KeyNum = c.Int(),
                    })
                .PrimaryKey(t => t.Type)
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID_AirplaneID)
                .ForeignKey("dbo.AirworthinessDirectives", t => t.AirworthinessDirectiveID_KeyNum)
                .Index(t => t.AirplaneID_AirplaneID)
                .Index(t => t.AirworthinessDirectiveID_KeyNum);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActiveAlerts", "AirworthinessDirectiveID_KeyNum", "dbo.AirworthinessDirectives");
            DropForeignKey("dbo.ActiveAlerts", "AirplaneID_AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.ActiveAlerts", new[] { "AirworthinessDirectiveID_KeyNum" });
            DropIndex("dbo.ActiveAlerts", new[] { "AirplaneID_AirplaneID" });
            DropTable("dbo.ActiveAlerts");
        }
    }
}
