namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EquipmentCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "EquipmentCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "EquipmentCode");
        }
    }
}
