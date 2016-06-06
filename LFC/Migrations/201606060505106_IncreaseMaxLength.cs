namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseMaxLength : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.BillingEmails");
            AlterColumn("dbo.BillingEmails", "Body", c => c.String(nullable: false, maxLength: 1024));
            AddPrimaryKey("dbo.BillingEmails", "Body");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.BillingEmails");
            AlterColumn("dbo.BillingEmails", "Body", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.BillingEmails", "Body");
        }
    }
}
