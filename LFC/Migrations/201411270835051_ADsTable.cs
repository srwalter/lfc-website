namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirworthinessDirectives",
                c => new
                    {
                        AirworthinessDirectiveID = c.String(nullable: false, maxLength: 128),
                        AirplaneID = c.String(maxLength: 128),
                        Description = c.String(),
                        FrequencyHours = c.Int(),
                        FrequencyMonths = c.Int(),
                        FrequencyMisc = c.String(),
                        LastDoneHours = c.Double(nullable: false),
                        LastDoneDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AirworthinessDirectiveID)
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirworthinessDirectives", new[] { "AirplaneID" });
            DropTable("dbo.AirworthinessDirectives");
        }
    }
}
