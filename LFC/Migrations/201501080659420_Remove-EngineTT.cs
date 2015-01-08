namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEngineTT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Airplanes", "EngineTT");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Airplanes", "EngineTT", c => c.Double(nullable: false));
        }
    }
}
