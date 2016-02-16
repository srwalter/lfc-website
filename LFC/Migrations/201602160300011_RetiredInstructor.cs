namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetiredInstructor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instructors", "Retired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instructors", "Retired");
        }
    }
}
