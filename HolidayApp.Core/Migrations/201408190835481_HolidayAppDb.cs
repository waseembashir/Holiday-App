namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holidays", "BookingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Holidays", "BookedBy", c => c.String());
            AddColumn("dbo.Holidays", "Holidaytype", c => c.String(nullable: false));
            AddColumn("dbo.Holidays", "HolidayDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holidays", "HolidayDescription");
            DropColumn("dbo.Holidays", "Holidaytype");
            DropColumn("dbo.Holidays", "BookedBy");
            DropColumn("dbo.Holidays", "BookingDate");
        }
    }
}
