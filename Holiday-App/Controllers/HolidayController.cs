using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;
using HolidayApp.Models;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity;

namespace HolidayApp.Controllers
{
    public class HolidayController : Controller
    {

       private HolidayAppDb db = new HolidayAppDb();

        // GET: /Holiday/
        
        
        // [Authorize] should use this helper attribute. This will force the user to login before they can 
        // View or book holidays. - WB
        [Authorize]
        public ActionResult Index()
        {

            var memberId = User.Identity.GetUserId();
           // Holiday holidays = db.MyHolidays.Find(memberId);
            
           //return View(db.Holidays.ToList());

           var loggedInUser = User.Identity.Name;
           var employee = db.GetEmployeeByUsername(loggedInUser);
           return View(db.GetHolidaysByEmployee(employee));

        }

        public ActionResult Create()
        {
            var memberId = User.Identity.GetUserId();
            ViewData["Message"] = memberId;
          
            return View();
        }

        // POST: /Employee/Create
        
        [HttpPost]

        public ActionResult Create([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee")] Holiday holiday)
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

        // GET: /Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return View(holiday);
        }

        // POST: /Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Holiday holiday = db.Holidays.Find(id);
            db.Holidays.Remove(holiday);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GEt: /Holiday/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }
            return View(holiday);

        }

        // POST: /Holiday/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StartDate,EndDate")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holiday).State = EntityState.Modified;
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holiday);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

	}
}