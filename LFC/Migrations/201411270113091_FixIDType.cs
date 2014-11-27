namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIDType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Equipments", new[] { "Airplane_AirplaneID" });
            DropColumn("dbo.Equipments", "AirplaneID");
            RenameColumn(table: "dbo.Equipments", name: "Airplane_AirplaneID", newName: "AirplaneID");
            AlterColumn("dbo.Equipments", "AirplaneID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Equipments", "AirplaneID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Equipments", new[] { "AirplaneID" });
            AlterColumn("dbo.Equipments", "AirplaneID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Equipments", name: "AirplaneID", newName: "Airplane_AirplaneID");
            AddColumn("dbo.Equipments", "AirplaneID", c => c.Int(nullable: false));
            CreateIndex("dbo.Equipments", "Airplane_AirplaneID");
        }
    }
}
