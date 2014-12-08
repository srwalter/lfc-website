namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BilledIsDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FlightLogs", "Billed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FlightLogs", "Billed", c => c.Boolean(nullable: false));
        }
    }
}
