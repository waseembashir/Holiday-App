using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using HolidayApp.Models;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using HolidayApp.Models;
using Microsoft.AspNet.Identity;

namespace HolidayApp.Controllers
{
    public class HolidayController : Controller
    {

       private HolidayAppDb db = new HolidayAppDb();

        // GET: /Holiday/
        public ActionResult Index()
        {
            var memberId = User.Identity.GetUserId();
            Holiday holidays = db.MyHolidays.Find(memberId);
            
           return View(db.MyHolidays.ToList());
        }

        public ActionResult Create()
        {
            var memberId = User.Identity.GetUserId();
            ViewData["Message"] = memberId;
          
            return View();
        }

        // POST: /Employee/Create
        
        [HttpPost]

        public ActionResult Create([Bind(Include = "EmployeeId,StartDate,EndDate,NoOfDays,UserId")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                db.MyHolidays.Add(holiday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        public ActionResult Details(int? id)
        {
            
            Holiday holiday = db.MyHolidays.Find(id);
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
            Holiday holiday = db.MyHolidays.Find(id);
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
            Holiday holiday = db.MyHolidays.Find(id);
            db.MyHolidays.Remove(holiday);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

	}
}