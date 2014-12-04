namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraACStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "GPSExprires", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "EngineSerial", c => c.String());
            AddColumn("dbo.Airplanes", "EngineTT", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "AirframeTT", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "AirframeTT");
            DropColumn("dbo.Airplanes", "EngineTT");
            DropColumn("dbo.Airplanes", "EngineSerial");
            DropColumn("dbo.Airplanes", "GPSExprires");
        }
    }
}
