namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HobbsViewModels",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        AirplaneID = c.String(nullable: false, maxLength: 128),
                        StartTach = c.Double(nullable: false),
                        EndTach = c.Double(nullable: false),
                        StartHobbs = c.Double(nullable: false),
                        EndHobbs = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.AirplaneID })
                .ForeignKey("dbo.Airplanes", t => t.AirplaneID, cascadeDelete: true)
                .Index(t => t.AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HobbsViewModels", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.HobbsViewModels", new[] { "AirplaneID" });
            DropTable("dbo.HobbsViewModels");
        }
    }
}
