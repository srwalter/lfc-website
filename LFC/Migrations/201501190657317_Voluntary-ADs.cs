namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoluntaryADs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AirworthinessDirectives", "Voluntary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AirworthinessDirectives", "Voluntary");
        }
    }
}
