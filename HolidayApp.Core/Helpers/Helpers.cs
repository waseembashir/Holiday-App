using HolidayApp.Core.Data;
using SalesFirst.Core.Model;
using System;
using System.Globalization;
using HolidayApp.Core.Model;
using System.Collections.Generic;
using System.Linq;
namespace HolidayApp.Core.Helpers
{
    public static class Helpers
    {
        /// 
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

        public static int TotalHolidaysTaken(Employee employee)
        {

            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();
            var weekends = 0;
            float total = 0;
            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;



                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                for (var i = 0; i <= days; i++)
                {
                    //total = (item.HalfDay == null) ? total+.5 : total++;
                    var testDate = startDate.AddDays(i);
                    switch (testDate.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            weekends++;
                            break;
                        case DayOfWeek.Sunday:
                            weekends++;
                            break;
                    }
                }


            }


            return weekends;


        }
    }
}
