namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holidays", "HalfDay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holidays", "HalfDay");
        }
    }
}
