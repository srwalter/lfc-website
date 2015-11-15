namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopilotAddIFR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Copilots", "InstrumentRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Copilots", "InstrumentRequired");
        }
    }
}
