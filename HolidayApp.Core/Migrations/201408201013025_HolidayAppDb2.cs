namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Holidays", "Holidaytype", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Holidays", "Holidaytype", c => c.String(nullable: false));
        }
    }
}
