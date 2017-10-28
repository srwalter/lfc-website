namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiamondApprovedInstructors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructors", "DiamondApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructors", "DiamondApproved");
        }
    }
}
