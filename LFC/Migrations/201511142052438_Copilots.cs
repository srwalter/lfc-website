namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Copilots : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Copilots",
                c => new
                    {
                        CopilotID = c.Int(nullable: false, identity: true),
                        ApplicationUserID = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.CopilotID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Copilots", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Copilots", new[] { "ApplicationUserID" });
            DropTable("dbo.Copilots");
        }
    }
}
