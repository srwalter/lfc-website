namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GPSTypo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "GPSExpires", c => c.DateTime(nullable: false));
            DropColumn("dbo.Airplanes", "GPSExprires");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Airplanes", "GPSExprires", c => c.DateTime(nullable: false));
            DropColumn("dbo.Airplanes", "GPSExpires");
        }
    }
}
