namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBillingEmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingEmails",
                c => new
                    {
                        Body = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Body);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillingEmails");
        }
    }
}
