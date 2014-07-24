namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Holidays");
            AddColumn("dbo.Holidays", "RequestId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Holidays", "EmployeeID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Holidays", "RequestId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Holidays");
            AlterColumn("dbo.Holidays", "EmployeeID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Holidays", "RequestId");
            AddPrimaryKey("dbo.Holidays", "EmployeeID");
        }
    }
}
