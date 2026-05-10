namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cellh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CellPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CellPhone");
        }
    }
}
