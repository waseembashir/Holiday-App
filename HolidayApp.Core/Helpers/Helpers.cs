using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayApp.Core.Helpers
{
    public class Helpers
    {
        public static DateTime IslamicToGeorgian(DateTime date)
        {
            /*NN: START Converts hijri(islamic date to georgian) */
            CultureInfo arCI = new CultureInfo("ar-SA");

            date = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", arCI.DateTimeFormat, DateTimeStyles.AllowInnerWhite);
            /*NN: END Converts hijri(islamic date to georgian) */
           
            return date;
        }

        public static DateTime SetYearToCurrent(DateTime date)
        {
            return new DateTime(DateTime.Now.Year, date.Month, date.Day);
        
        
        }
    }
}
