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

    }

}