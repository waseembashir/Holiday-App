namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HolidayDescriptions", "TypeFor", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HolidayDescriptions", "TypeFor");
        }
    }
}
