﻿using HolidayApp.Core.Data;
using SalesFirst.Core.Model;
using System;
using System.Globalization;
using HolidayApp.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using SalesFirst.Core.Service;
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

        public static Dictionary<string, string> GetHolidayNamesAndColors()
        {
           
            HolidayAppDb db = new HolidayAppDb();
            List<HolidayDescription> list = db.GetAllHolidayDescriptions.ToList();

            Dictionary<string, string> groups = new Dictionary<string, string>();
            foreach (var holidayType in list)
            {
                groups.Add(holidayType.HolidayType,holidayType.HolidayColor);
            }

            return groups;
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
        public static float HolidaysTaken(Employee employee)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();


            float result = list.Select(c => c.NoOfDays).Sum();
            
            return result;
        }

        /// <summary>
        /// gets holidays booked in particular year
        /// </summary>
        /// <param name="employee">employee</param>
        /// <param name="FromDate">from date of type datetime</param>
        /// <returns>returns number of holidays of type int booked in particular year</returns>
        public static float HolidaysBookedInYear(Employee employee, int Year)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();
            List<Holiday> newlist = Helpers.HolidayListOfYear(employee,Year);
            float result = newlist.Select(c => c.NoOfDays).Sum();

            return result;

        }

        /// <summary>
        /// gets different types of holiday types and corresponding number of total days of dieefernt holiday types
        /// </summary>
        /// <param name="username">username of employee</param>
        /// <returns>holiday type and number of days</returns>
        public static Dictionary<string, float> HolidayTypesTaken(String username)
        {
            HolidayAppDb db = new HolidayAppDb();
            EmployeeService employeeService = new EmployeeService(new SalesFirst.Core.Data.EmployeeRepository(new HolidayApp.Core.Data.HolidayAppDb()));
            SalesFirst.Core.Model.Employee employee = employeeService.GetEmployeeByUsername(username);
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();

            var query = list.GroupBy(n => n.Holidaytype,
                    (key, values) => new { Group = key, Count = values.Count() });


            var result = list.GroupBy(h => h.Holidaytype)
                          .Select(hd =>
                                new
                                {
                                    Group = hd.Key,
                                    Count = hd.Count(),
                                    Sum = hd.Sum(h => h.NoOfDays)
                                });


            Dictionary<string, float> groups =  new Dictionary<string, float>();
            foreach(var item in result)
            {
                groups.Add(item.Group,item.Sum);

            }



            return groups;
        }

        /// <summary>
        /// gets different types of holiday types and corresponding number of total days of dieefernt holiday types in year
        /// </summary>
        /// <param name="username">username of employee</param>
        /// <returns>holiday type and number of days in year</returns>
        public static Dictionary<string, float> HolidayTypesTakenInYear(String username, int Year)
        {
            HolidayAppDb db = new HolidayAppDb();
            EmployeeService employeeService = new EmployeeService(new SalesFirst.Core.Data.EmployeeRepository(new HolidayApp.Core.Data.HolidayAppDb()));
            SalesFirst.Core.Model.Employee employee = employeeService.GetEmployeeByUsername(username);
            List<Holiday> list = Helpers.HolidayListOfYear(employee, Year);

            var query = list.GroupBy(n => n.Holidaytype,
                    (key, values) => new { Group = key, Count = values.Count() });


            var result = list.GroupBy(h => h.Holidaytype)
                          .Select(hd =>
                                new
                                {
                                    Group = hd.Key,
                                    Count = hd.Count(),
                                    Sum = hd.Sum(h => h.NoOfDays)
                                });


            Dictionary<string, float> groups = new Dictionary<string, float>();
            foreach (var item in result)
            {
                groups.Add(item.Group, item.Sum);

            }



            return groups;
        }

        /// <summary>
        /// gets list of individual holidays in particular year, and adjusting halfday count
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="Year">returns list of individual holidays in particular year, and adjusting halfday count</param>
        /// <returns></returns>
        public static List<Holiday> HolidayListOfYear(Employee employee, int Year)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetApprovedHolidaysByEmployee(employee).ToList();
            List<Holiday> newlist = new List<Holiday>();
           
            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;

                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
               bool flag = false;
                for (var i = 0; i <= days; i++)
                {
                    //total = (item.HalfDay == null) ? total+.5 : total++;
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == Year)
                    {
                        Holiday holiday = new Holiday();
                        
                        holiday.BookedBy = item.BookedBy;
                        holiday.BookingDate = item.BookingDate;
                        holiday.Employee = item.Employee;
                        holiday.EndDate = testDate;
                        holiday.HolidayDescription = item.HolidayDescription;
                        holiday.HolidayId = item.HolidayId;
                        holiday.Holidaytype = item.Holidaytype;
                        holiday.NoOfDays = 1;
                        holiday.StartDate = testDate;
                        holiday.Status = item.Status;
                        holiday.HalfDay = item.HalfDay;
                        if (holiday.HalfDay != null && flag == false)
                        {
                        holiday.NoOfDays = holiday.NoOfDays / 2;
                        flag = true;
                        }
                        newlist.Add(holiday);
                    }

                }
               
                

            }

            return newlist;

        }


        public static List<CombinedHoliday> CompleteHolidayListOfYear(int Year)
        {
            HolidayAppDb db = new HolidayAppDb();
            List<Holiday> list = db.GetNotRejectedHolidays().ToList();
            List<CombinedHoliday> newlist = new List<CombinedHoliday>().ToList();

            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;

                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                bool flag = false;
                for (var i = 0; i <= days; i++)
                {var t= i;
                    //total = (item.HalfDay == null) ? total+.5 : total++;
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == Year)
                    {
                        CombinedHoliday holiday = new CombinedHoliday();
                        holiday.day = 1;
                        if (item.HalfDay != null && flag == false && t == days)
                        {
                            holiday.day = holiday.day / 2;
                            flag = true;
                        }
                        holiday.name = item.Employee.Username;
                        holiday.date = testDate;
                        holiday.type = item.Holidaytype;
                        holiday.status = item.Status;
                        
                        newlist.Add(holiday);
                    }

                }



            }

            List<GeneralHoliday> list2 = db.GetAllGeneralHolidays.ToList();
            foreach (var item in list2)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                if (item.Type == "islamic-public-holiday") { 
                startDate = Helpers.IslamicToGeorgian(item.StartDate);
                endDate = Helpers.IslamicToGeorgian(item.EndDate);
                }
                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                bool flag = false;
                for (var i = 0; i <= days; i++)
                {
                    //total = (item.HalfDay == null) ? total+.5 : total++;
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == Year)
                    {
                        CombinedHoliday holiday = new CombinedHoliday();

                        holiday.name = item.Name;
                        holiday.date = testDate;
                        holiday.type = item.Type;
                        holiday.status = item.Name;
                        holiday.day = 1;
                        
                        newlist.Add(holiday);
                    }

                }



            }


            return newlist.OrderBy(o => o.date).ToList();

        }




    }
    public class CombinedHoliday
    {
        public string name;
        public DateTime date;
        public string status;
        public float day;
        public string type;
    }
}
