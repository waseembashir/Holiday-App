using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HolidayApp.Validations;
using SalesFirst.Core.Model;

namespace HolidayApp.Core.Model
{
    public class Holiday
    {
  
        [Key]
        public virtual int HolidayId{ get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CurrentDateCheck]
        public virtual DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [HolidayValidations("StartDate")]
        public virtual DateTime EndDate { get; set; }
 
        [Required]
        [Range(0.5,100)]
        [DiffrenceInDays("StartDate","EndDate")]
        public virtual float NoOfDays { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual String Status { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime BookingDate { get; set; }

        public virtual String BookedBy { get; set; }

        [Required]
        public virtual String Holidaytype { get; set; }

        public virtual String HolidayDescription { get; set; }
        
    }
}
