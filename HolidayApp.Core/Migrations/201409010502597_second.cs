namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolidayDescriptions",
                c => new
                    {
                        HolidayDescriptionId = c.Int(nullable: false, identity: true),
                        HolidayType = c.String(nullable: false),
                        HolidayColor = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.HolidayDescriptionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HolidayDescriptions");
        }
    }
}
