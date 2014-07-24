namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidayAppDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        NoOfDays = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeID);
                
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Holidays");
        }
    }
}
