using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HolidayApp.Validations
{
    public class HolidayValidations: ValidationAttribute
    {
        String OtherPropertyName;
        public HolidayValidations(string otherPropertyName)
            :base("{0} shoud be greater or equal to {1}")
        {
            OtherPropertyName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name, OtherPropertyName);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            /*Disabled this code as it failed to get the value object as it was null - WB.*/

            var otherpropertyinfo = validationContext.ObjectType.GetProperty(OtherPropertyName);
            var Otherdate = (DateTime)otherpropertyinfo.GetValue(validationContext.ObjectInstance, null);
            var thisdate = (DateTime)value;

            if (thisdate < Otherdate)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }

            return null;
        }
    }

    public class CurrentDateCheck:ValidationAttribute
    {
        
        public CurrentDateCheck()
            :base("{0} shoud be Today's Date or Greater")
        {
           
        }
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var thisdate = (DateTime)value;
            var today = DateTime.Today;

            if(thisdate < today)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }

            return null;
        }
    }

    //Check whether difference between two dates is proper if javascript id disabled

    public class DiffrenceInDays : ValidationAttribute
    {
        public String d1 { get; set; }
        public String d2 { get; set; }
        public DiffrenceInDays(String startdate, String endate)
            :base("Number of days should be differece between startdate and EndDate")
        {
            d1 = startdate;
            d2 = endate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var start = validationContext.ObjectType.GetProperty(d1); //get startdate
            var end = validationContext.ObjectType.GetProperty(d2);  // get enddate 
            var Date1 = (DateTime)start.GetValue(validationContext.ObjectInstance, null);
            var Date2 = (DateTime)end.GetValue(validationContext.ObjectInstance, null);
            var Days = (float)value;
            Double diff;
            if(Date1==Date2)
            {
                diff = 0.5;
                
            }
            else
            {
                diff = (Date2 - Date1).TotalDays;
            }
            if (Days != diff)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }

            return null;
        }
    }

}