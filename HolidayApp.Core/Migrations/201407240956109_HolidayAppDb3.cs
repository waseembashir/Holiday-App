namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holidays", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holidays", "UserId");
        }
    }
}
