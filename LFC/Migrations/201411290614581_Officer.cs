namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Officer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Officer", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Officer");
        }
    }
}
