namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEquipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentID = c.Int(nullable: false, identity: true),
                        AirplaneID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                        Airplane_AirplaneID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EquipmentID)
                .ForeignKey("dbo.Airplanes", t => t.Airplane_AirplaneID)
                .Index(t => t.Airplane_AirplaneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Equipments", "Airplane_AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.Equipments", new[] { "Airplane_AirplaneID" });
            DropTable("dbo.Equipments");
        }
    }
}
