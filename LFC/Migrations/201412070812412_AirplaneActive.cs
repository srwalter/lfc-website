namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirplaneActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "Active");
        }
    }
}
