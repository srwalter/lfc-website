namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompoundKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirworthinessDirectives", new[] { "AirplaneID" });
            DropPrimaryKey("dbo.AirworthinessDirectives");
            AlterColumn("dbo.AirworthinessDirectives", "AirplaneID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AirworthinessDirectives", new[] { "AirplaneID", "AirworthinessDirectiveID" });
            CreateIndex("dbo.AirworthinessDirectives", "AirplaneID");
            AddForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirworthinessDirectives", new[] { "AirplaneID" });
            DropPrimaryKey("dbo.AirworthinessDirectives");
            AlterColumn("dbo.AirworthinessDirectives", "AirplaneID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.AirworthinessDirectives", "AirworthinessDirectiveID");
            CreateIndex("dbo.AirworthinessDirectives", "AirplaneID");
            AddForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes", "AirplaneID");
        }
    }
}
