using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using HolidayApp.Models;

namespace HolidayApp.Controllers
{
    public class HolidayController : Controller
    {

       private HolidayAppDb db = new HolidayAppDb();

        // GET: /Holiday/
        
        
        // [Authorize] should use this helper attribute. This will force the user to login before they can 
        // View or book holidays. - WB
        public ActionResult Index()
        {
           var loggedInUser = User.Identity.Name;
           var employee = db.GetEmployeeByUsername(loggedInUser);
           return View(db.GetHolidaysByEmployee(employee));
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        
        [HttpPost]

        public ActionResult Create([Bind(Include = "EmployeeId,StartDate,EndDate,NoOfDays")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = db.GetEmployeeByUsername(loggedInUser);
            
            // Add validation for dates here.

            if (ModelState.IsValid)
            {
                holiday.Employee = employee;
                db.Holidays.Add(holiday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        public ActionResult Details(int? id)
        {

            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return View(holiday);  
        }
	}
}