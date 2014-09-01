using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayApp.Core.Model
{
    public class HolidayDescription
    {

        [Key]
        public virtual int HolidayDescriptionId { get; set; }
        
        [Display(Name = "Holiday Type Name")]
        [Required]
        public string HolidayType { get; set; }

        [Display(Name = "Holiday Type Color")]
        [Required]
        public string HolidayColor { get; set; }

        [Display(Name = "Holiday Type For")]
        [Required]
        public virtual TypeFor? TypeFor { get; set; }

    }
    public enum TypeFor{
       GeneralCalendar,
       EmployeeHolidays

    }
}
