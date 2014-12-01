namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlightLogs",
                c => new
                    {
                        FlightLogID = c.Int(nullable: false, identity: true),
                        AirplaneID = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        StartTach = c.Double(nullable: false),
                        EndTach = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.FlightLogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FlightLogs");
        }
    }
}
