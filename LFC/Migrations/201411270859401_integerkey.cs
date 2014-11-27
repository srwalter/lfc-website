namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integerkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirworthinessDirectives", new[] { "AirplaneID" });
            DropPrimaryKey("dbo.AirworthinessDirectives");
            AddColumn("dbo.AirworthinessDirectives", "KeyNum", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AirworthinessDirectives", "AirplaneID", c => c.String(maxLength: 128));
            AlterColumn("dbo.AirworthinessDirectives", "AirworthinessDirectiveID", c => c.String());
            AddPrimaryKey("dbo.AirworthinessDirectives", "KeyNum");
            CreateIndex("dbo.AirworthinessDirectives", "AirplaneID");
            AddForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes", "AirplaneID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes");
            DropIndex("dbo.AirworthinessDirectives", new[] { "AirplaneID" });
            DropPrimaryKey("dbo.AirworthinessDirectives");
            AlterColumn("dbo.AirworthinessDirectives", "AirworthinessDirectiveID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AirworthinessDirectives", "AirplaneID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AirworthinessDirectives", "KeyNum");
            AddPrimaryKey("dbo.AirworthinessDirectives", new[] { "AirplaneID", "AirworthinessDirectiveID" });
            CreateIndex("dbo.AirworthinessDirectives", "AirplaneID");
            AddForeignKey("dbo.AirworthinessDirectives", "AirplaneID", "dbo.Airplanes", "AirplaneID", cascadeDelete: true);
        }
    }
}
