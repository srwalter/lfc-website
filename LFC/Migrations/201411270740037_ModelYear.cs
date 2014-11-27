namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "ModelYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "ModelYear");
        }
    }
}
