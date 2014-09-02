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
        /// <summary>
        /// Maps islamic date to georgian equivalent
        /// </summary>
        /// <param name="date">takes param of type datetime as inptu</param>
        /// <returns>return georgian equivalent of type datetime</returns>
        public static DateTime IslamicToGeorgian(DateTime date)
        {
            /*NN: START Converts hijri(islamic date to georgian) */
            CultureInfo arCI = new CultureInfo("ar-SA");

            date = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", arCI.DateTimeFormat, DateTimeStyles.AllowInnerWhite);
            /*NN: END Converts hijri(islamic date to georgian) */

            return date;
        }
        /// <summary>
        /// Returns list of css classes defining holiday type colors
        /// </summary>
        /// <returns>Returns string of css classes defining holiday type colors</returns>
        public static string HolidayColors()
        {
            string cssclass = "";
            string css = "";
            HolidayAppDb db = new HolidayAppDb();
            List<HolidayDescription> list = db.GetAllHolidayDescriptions.ToList();
            foreach (var holidayType in list)
            {
                cssclass = "#year-calendar ." + holidayType.HolidayType + "{";
                cssclass += "background-color:" + holidayType.HolidayColor + ";";
                cssclass += "color: white;}";
                css += cssclass;
            }

            return css;
        }
        /// <summary>
        /// Returns color of holiday type by passing in holiday type
        /// </summary>
        /// <param name="HolidayType">takes holiday type name as input of type string</param>
        /// <returns>Returns string(color name) of holiday type by passing in holiday type</returns>
        public static string GetHolidayTypeColor(string HolidayType)
        {
            string color = "";
            HolidayAppDb db = new HolidayAppDb();
            List<HolidayDescription> list = db.GetAllHolidayDescriptions.ToList();
            foreach (var holidayType in list)
            {
                if (holidayType.HolidayType == HolidayType)
                    color = holidayType.HolidayColor;

            }

            return color;
        }

        /// <summary>
        /// Sets year to current of passed in date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime SetYearToCurrent(DateTime date)
        {
            return new DateTime(DateTime.Now.Year, date.Month, date.Day);


        }

        /// <summary>
        /// deducts weekends or general public holidays from total holidays taken by user incase user accidently books one of those dates
        /// </summary>
        /// <param name="employee">takes employee as input</param>
        /// <returns>retuns number of holidays of type int</returns>
        public static int ActualHolidaysTaken(Employee employee)
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

        /// <summary>
        /// checks total holidays entitlement of particular employee
        /// </summary>
        /// <param name="employee">takes employee as input</param>
        /// <returns>returns number of total entitlements of type int </returns>
        public static int TotalEntitlement(Employee employee)
        {
            HolidayAppDb db = new HolidayAppDb();
            EmployeeQuota Quotas = db.GetEmployeeQuotaByEmployee(employee);

            int result = Quotas.PaidQuota;
            return result;
        }

        /// <summary>
        /// checks total holidays taken by particular employee
        /// </summary>
        /// <param name="employee">takes employee as input</param>
        /// <returns>returns number of holidays of type int taken by particular employee</returns>
        public static int HolidaysTaken(Employee employee)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();

            int result = list.Count;
            return result;
        }

        /// <summary>
        /// gets holidays booked in particular year
        /// </summary>
        /// <param name="employee">employee</param>
        /// <param name="FromDate">from date of type datetime</param>
        /// <returns>returns number of holidays of type int booked in particular year</returns>
        public static int HolidaysBookedInYear(Employee employee, int Year)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();

            int result = 30;
            return result;
        }

    }
}
