using System.Data.Entity;
using System.Linq;
using HolidayApp.Core.Model;
using SalesFirst.Core.Model;
using SalesFirst.Core.Data;

namespace HolidayApp.Core.Data
{
    /*NN: Inherited from ClientDb, so extending previous context and usng that single context all over app  NN. */
    public class HolidayAppDb : ClientDb
    {



        /*NN: ALready in ClientDb context*/
        //public DbSet<Employee> Employees { get; set; }

        //public IQueryable<Employee> GetAllEmployees
        //{
        //    get { return Employees; }
        //}

        public DbSet<Holiday> Holidays { get; set; }

        public IQueryable<Holiday> GetAllHolidays
        {
            get { return Holidays; }
        }

        public IQueryable<Holiday> GetHolidaysByEmployee(Employee employee)
        {
           return Holidays.Where(r => r.Employee.Username == employee.Username);
        }

        //public Employee GetEmployeeByUsername(string username)
        //{
            
        //    return Employees.FirstOrDefault(r => r.Username == username);
        //}

    }


}