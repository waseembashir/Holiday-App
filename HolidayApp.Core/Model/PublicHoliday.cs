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
    public class PublicHoliday
    {
        [Key]
        public virtual int PublicHolidayId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Type? Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [HolidayValidations("StartDate")]
        public virtual DateTime EndDate { get; set; }

        [Required]
        [Range(0.5, 100)]

        public virtual float NoOfDays { get; set; }


        public virtual Frequency? Frequency { get; set; }

    }
 
    public enum Type
    {
        Islamic,
        General

    }
}
