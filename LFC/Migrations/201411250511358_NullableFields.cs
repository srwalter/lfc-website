namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Certificate", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "MemberType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "MemberType", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Certificate", c => c.Int(nullable: false));
        }
    }
}
