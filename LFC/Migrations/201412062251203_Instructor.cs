namespace LFC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Instructor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructors",
                c => new
                    {
                        InstructorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CFII = c.Boolean(nullable: false),
                        MEI = c.Boolean(nullable: false),
                        ASEL = c.Boolean(nullable: false),
                        ASES = c.Boolean(nullable: false),
                        OtherRatings = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                        DayPhone = c.String(),
                        EveningPhone = c.String(),
                        CellPhone = c.String(),
                        Email = c.String(),
                        Available = c.String(),
                    })
                .PrimaryKey(t => t.InstructorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Instructors");
        }
    }
}
