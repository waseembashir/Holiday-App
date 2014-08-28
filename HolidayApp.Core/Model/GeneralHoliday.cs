﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HolidayApp.Validations;
using SalesFirst.Core.Model;
using System.ComponentModel;

namespace HolidayApp.Core.Model
{
    public class GeneralHoliday
    {
        [Key]
        public virtual int GeneralHolidayId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

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
        public virtual float NoOfDays { 
            get { return (float)EndDate.Subtract(StartDate).TotalDays;
         
            }
            
            set{} }


        public virtual Frequency? Frequency { get; set; }

    }
    public enum Frequency
    {
        Yearly,
        YearSpecific
       
    }
}