namespace HolidayApp.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Budget = c.Single(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Cost = c.Double(),
                        Balance = c.Double(),
                        Source = c.String(),
                        ConversionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        HarvestInvoiceId = c.Long(),
                        Client_Id = c.Int(),
                        Domain_Id = c.Int(),
                        HostingPackage_Id = c.Int(),
                        SalesPerson_EmployeeId = c.Int(),
                        Employee_EmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .ForeignKey("dbo.DomainOrders", t => t.Domain_Id)
                .ForeignKey("dbo.Hostings", t => t.HostingPackage_Id)
                .ForeignKey("dbo.Employees", t => t.SalesPerson_EmployeeId)
                .ForeignKey("dbo.Employees", t => t.Employee_EmployeeId)
                .Index(t => t.Client_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.HostingPackage_Id)
                .Index(t => t.SalesPerson_EmployeeId)
                .Index(t => t.Employee_EmployeeId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ContactNumber = c.String(nullable: false),
                        Mobile = c.String(),
                        EmailId = c.String(),
                        Address = c.String(),
                        Status = c.String(),
                        ResellerClientKey = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        UpdatedTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        ClientIndustry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientIndustries", t => t.ClientIndustry_Id)
                .Index(t => t.ClientIndustry_Id);
            
            CreateTable(
                "dbo.CallLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Action = c.String(),
                        Reminder = c.DateTime(precision: 7, storeType: "datetime2"),
                        Status = c.String(),
                        WelcomeEmail = c.String(),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Datetime = c.DateTime(nullable: false),
                        Outcome = c.String(),
                        Remarks = c.String(),
                        CallLog_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CallLogs", t => t.CallLog_Id)
                .Index(t => t.CallLog_Id);
            
            CreateTable(
                "dbo.ClientIndustries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DomainOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResellerKey = c.Int(nullable: false),
                        Url = c.String(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Registered = c.DateTime(nullable: false),
                        Expiry = c.DateTime(nullable: false),
                        Status = c.String(),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Hostings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ToDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Cost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeType", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Project_Id", c => c.Int());
            CreateIndex("dbo.Employees", "Project_Id");
            AddForeignKey("dbo.Employees", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Employee_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Projects", "SalesPerson_EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "HostingPackage_Id", "dbo.Hostings");
            DropForeignKey("dbo.Projects", "Domain_Id", "dbo.DomainOrders");
            DropForeignKey("dbo.Projects", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.DomainOrders", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Clients", "ClientIndustry_Id", "dbo.ClientIndustries");
            DropForeignKey("dbo.Meetings", "CallLog_Id", "dbo.CallLogs");
            DropForeignKey("dbo.CallLogs", "Client_Id", "dbo.Clients");
            DropIndex("dbo.DomainOrders", new[] { "Client_Id" });
            DropIndex("dbo.Meetings", new[] { "CallLog_Id" });
            DropIndex("dbo.CallLogs", new[] { "Client_Id" });
            DropIndex("dbo.Clients", new[] { "ClientIndustry_Id" });
            DropIndex("dbo.Projects", new[] { "Employee_EmployeeId" });
            DropIndex("dbo.Projects", new[] { "SalesPerson_EmployeeId" });
            DropIndex("dbo.Projects", new[] { "HostingPackage_Id" });
            DropIndex("dbo.Projects", new[] { "Domain_Id" });
            DropIndex("dbo.Projects", new[] { "Client_Id" });
            DropIndex("dbo.Employees", new[] { "Project_Id" });
            DropColumn("dbo.Employees", "Project_Id");
            DropColumn("dbo.Employees", "EmployeeType");
            DropColumn("dbo.Employees", "Gender");
            DropTable("dbo.Hostings");
            DropTable("dbo.DomainOrders");
            DropTable("dbo.ClientIndustries");
            DropTable("dbo.Meetings");
            DropTable("dbo.CallLogs");
            DropTable("dbo.Clients");
            DropTable("dbo.Projects");
        }
    }
}
