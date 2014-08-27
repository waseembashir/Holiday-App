﻿using System.Data.Entity;
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
        public IQueryable<Holiday> GetAllHolidays
        {
            get { return Holidays; }
        }
        public IQueryable<GeneralHoliday> GetAllGeneralHolidays
        {
            get { return GeneralHolidays; }
        }

        public IQueryable<Holiday> GetHolidaysByEmployee(Employee employee)
        {
            return Holidays.Where(r => r.Employee.Username == employee.Username);
        }

        public IQueryable<EmployeeQuota> GetAllEmployeeQuotas
        {
            get { return EmployeeQuotas; }
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