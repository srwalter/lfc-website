namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Billed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlightLogs", "Billed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlightLogs", "Billed");
        }
    }
}
