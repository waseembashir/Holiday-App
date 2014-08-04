namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
           
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        HireDate = c.DateTime(nullable: false),
                        TerminationDate = c.DateTime(),
                        Address = c.String(),
                        ContactNumber = c.Long(nullable: false),
                        PersonalEmail = c.String(),
                        Username = c.String(),
                        JobTitle = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        HolidayId = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        NoOfDays = c.Int(nullable: false),
                        Status = c.String(),
                        Employee_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.HolidayId)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId)
                .Index(t => t.Employee_EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Holidays", "Employee_EmployeeId", "dbo.Employees");
            DropIndex("dbo.Holidays", new[] { "Employee_EmployeeId" });
            DropTable("dbo.Holidays");
            DropTable("dbo.Employees");
        }
    }
}
