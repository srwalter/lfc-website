namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAirframeTT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Airplanes", "AirframeTT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Airplanes", "AirframeTT", c => c.Double(nullable: false));
        }
    }
}
