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
        public ActionResult Index()
        {
            
           ViewData["Message"] ="aa" ;
           return View(db.MyHolidays.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        
        [HttpPost]

        public ActionResult Create([Bind(Include = "EmployeeId,StartDate,EndDate,NoOfDays")] Holiday holiday)
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
	}
}