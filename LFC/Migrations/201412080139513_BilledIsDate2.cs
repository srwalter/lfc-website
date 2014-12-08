namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BilledIsDate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlightLogs", "Billed", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlightLogs", "Billed");
        }
    }
}
