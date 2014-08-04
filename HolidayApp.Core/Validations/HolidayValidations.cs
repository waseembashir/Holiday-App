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
            var otherpropertyinfo = validationContext.ObjectType.GetProperty(OtherPropertyName);
            var Otherdate = (DateTime)otherpropertyinfo.GetValue(validationContext.ObjectInstance, null);
            var thisdate = (DateTime)value;

            if(thisdate > Otherdate)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }

            return null;
        }
    }
}