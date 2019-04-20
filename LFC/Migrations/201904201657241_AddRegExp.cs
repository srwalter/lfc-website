namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegExp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Airplanes", "RegistrationExpires", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Airplanes", "RegistrationExpires");
        }
    }
}
