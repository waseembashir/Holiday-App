using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HolidayApp.Core.Model
{
    public class Holiday
    {
  
        //public virtual int EmployeeID { get; set; }
       
        [Key]
        public virtual int RequestId{get;set;}

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime EndDate { get; set; }

       [Required]
        public virtual int NoOfDays { get; set; }

       public virtual String UserId { get; set; }

        public virtual String Status { get; set; }
    }
}
