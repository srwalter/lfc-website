namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HobbsTimes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HobbsTimes",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AirplaneID = c.String(nullable: false, maxLength: 128),
                        HobbsHours = c.Double(nullable: false),
                        TachHours = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.AirplaneID })
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID, cascadeDelete: true)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HobbsTimes", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.HobbsTimes", new[] { "AirplaneID" });
            DropTable("dbo.HobbsTimes");
        }
    }
}
