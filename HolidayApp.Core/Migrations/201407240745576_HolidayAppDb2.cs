namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb2 : DbMigration
    {
        public override void Up()
        {
          //  DropColumn("dbo.Holidays", "EmployeeID");
        }
        
        public override void Down()
        {
           // AddColumn("dbo.Holidays", "EmployeeID", c => c.Int(nullable: false));
        }
    }
}
