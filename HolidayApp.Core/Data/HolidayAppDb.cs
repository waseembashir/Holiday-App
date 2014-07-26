using System.Data.Entity;
using System.Linq;
using HolidayApp.Core.Model;

namespace HolidayApp.Core.Data
{
    public class HolidayAppDb : DbContext
    {

        
        public HolidayAppDb()
            : base("DefaultConnection")
        {

        }
        
        public DbSet<Employee> Employees { get; set; }

        public IQueryable<Employee> GetAllEmployees
        {
            get { return Employees; }
        }

        public DbSet<Holiday> Holidays { get; set; }

        public IQueryable<Holiday> GetAllHolidays
        {
            get { return Holidays; }
        }

        public IQueryable<Holiday> GetHolidaysByEmployee(Employee employee)
        {
            return Holidays.Where(r => r.Employee == employee);
        }

        public Employee GetEmployeeByUsername(string username)
        {
            return Employees.FirstOrDefault(r => r.Username == username);
        }

    }


}