namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightLogKey2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.FlightLogs");
            AlterColumn("dbo.FlightLogs", "FlightLogID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.FlightLogs", "FlightLogID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.FlightLogs");
            AlterColumn("dbo.FlightLogs", "FlightLogID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.FlightLogs", "FlightLogID");
        }
    }
}
