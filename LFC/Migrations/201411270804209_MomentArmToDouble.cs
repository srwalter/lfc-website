namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MomentArmToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Airplanes", "Moment", c => c.Double(nullable: false));
            AlterColumn("dbo.Airplanes", "Arm", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Airplanes", "Arm", c => c.Single(nullable: false));
            AlterColumn("dbo.Airplanes", "Moment", c => c.Single(nullable: false));
        }
    }
}
