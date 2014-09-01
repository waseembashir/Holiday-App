using System.Data.Entity;
using System.Linq;
using HolidayApp.Core.Model;
using SalesFirst.Core.Model;
using SalesFirst.Core.Data;
using System;

namespace HolidayApp.Core.Data
{
    /*NN: Inherited from ClientDb, so extending previous context and usng that single context all over app  NN. */
    public class HolidayAppDb : ClientDb
    {

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<GeneralHoliday> GeneralHolidays { get; set; }
        public DbSet<EmployeeQuota> EmployeeQuotas { get; set; }
        public DbSet<HolidayDescription> HolidayDescriptions { get; set; }
        public IQueryable<Holiday> GetAllHolidays
        {
            get { return Holidays; }
        }
        public IQueryable<HolidayDescription> GetAllHolidayDescriptions
        {
            get { return HolidayDescriptions; }
        }
        public IQueryable<HolidayDescription> GetAllEmployeeHolidayDescriptions
        {
            get { return HolidayDescriptions.Where(r => r.TypeFor == TypeFor.EmployeeHolidays && r.HolidayType != "pending"); }
        }
        public IQueryable<HolidayDescription> GetAllGeneralHolidayDescriptions
        {
            get { return HolidayDescriptions.Where(r => r.TypeFor == TypeFor.GeneralCalendar && r.HolidayType != "pending"); }
        }
     
        public IQueryable<GeneralHoliday> GetAllGeneralHolidays
        {
            get { return GeneralHolidays; }
        }

        public IQueryable<Holiday> GetHolidaysByEmployee(Employee employee)
        {
            return Holidays.Where(r => r.Employee.Username == employee.Username);
        }
        public IQueryable<Holiday> GetNotRejectedHolidaysByEmployee(Employee employee)
        {
            return Holidays.Where(r => r.Employee.Username == employee.Username && r.Status != "Rejected");
        }
        public IQueryable<Holiday> GetApprovedHolidaysByEmployee(Employee employee)
        {
            return Holidays.Where(r => r.Employee.Username == employee.Username && r.Status == "Approved");
        }


        public IQueryable<EmployeeQuota> GetAllEmployeeQuotas
        {
            get { return EmployeeQuotas; }
        }

        public EmployeeQuota GetEmployeeQuotaByEmployee(Employee employee)
        {
            return EmployeeQuotas.Where(r => r.EmployeeId == employee.EmployeeId).SingleOrDefault();
        }


        public IQueryable<Holiday> GetApprovedHolidays()
        {
            return Holidays.Where(r => r.Status == "Approved");
           
        }


        public IQueryable<Holiday> GetDisApprovedHolidays()
        {
            return Holidays.Where(r => r.Status == "Rejected" || r.StartDate < DateTime.Today);

        }

        public IQueryable<Holiday> GetPendingHolidays()
        {
           
            return Holidays.Where(r => r.Status ==null && r.StartDate >=  DateTime.Today);

        }





    }


}