namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalZip : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instructors", "ZipCode", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Instructors", "ZipCode", c => c.Int(nullable: false));
        }
    }
}
