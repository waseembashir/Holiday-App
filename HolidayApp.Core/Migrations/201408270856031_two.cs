namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneralHolidays", "Frequency", c => c.Int());
            DropColumn("dbo.GeneralHolidays", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralHolidays", "Status", c => c.Int());
            DropColumn("dbo.GeneralHolidays", "Frequency");
        }
    }
}
