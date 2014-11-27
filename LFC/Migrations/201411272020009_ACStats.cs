namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ACStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "TachAdd", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "EngineOverhaul", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "CurrentTach", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "HundredHour", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "OilChange", c => c.Double(nullable: false));
            AddColumn("dbo.Airplanes", "AnnualDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "EltDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "EltBatteryDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "TransponderDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "StaticDue", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "Updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Airplanes", "UpdatedBy", c => c.String());
            AddColumn("dbo.Airplanes", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "Comments");
            DropColumn("dbo.Airplanes", "UpdatedBy");
            DropColumn("dbo.Airplanes", "Updated");
            DropColumn("dbo.Airplanes", "StaticDue");
            DropColumn("dbo.Airplanes", "TransponderDue");
            DropColumn("dbo.Airplanes", "EltBatteryDue");
            DropColumn("dbo.Airplanes", "EltDue");
            DropColumn("dbo.Airplanes", "AnnualDue");
            DropColumn("dbo.Airplanes", "OilChange");
            DropColumn("dbo.Airplanes", "HundredHour");
            DropColumn("dbo.Airplanes", "CurrentTach");
            DropColumn("dbo.Airplanes", "EngineOverhaul");
            DropColumn("dbo.Airplanes", "TachAdd");
        }
    }
}
