namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Holidays", "NoOfDays", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Holidays", "NoOfDays", c => c.Int(nullable: false));
        }
    }
}
