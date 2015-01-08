namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComputeCurrentTach : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Airplanes", "CurrentTach");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Airplanes", "CurrentTach", c => c.Double(nullable: false));
        }
    }
}
