namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HobbsTimeBilled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HobbsTimes", "Billed", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HobbsTimes", "Billed");
        }
    }
}
