using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using SalesFirst.Core.Data;
using System.Globalization;
using SalesFirst.Core.Service;
namespace HolidayApp.Controllers
{
    [Authorize]
    public class GeneralHolidayController : Controller
    {
        private HolidayAppDb db = new HolidayAppDb();

         readonly EmployeeRepository employeeRepository;
       readonly EmployeeService employeeService;

       public GeneralHolidayController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }


        // GET: /General/
        public ActionResult Index()
        {
            return View(db.GeneralHolidays.ToList());
        }
        public ActionResult Calendar()
        {
            return View(db.GeneralHolidays.ToList());
        }
        public ActionResult FullCalendar()
        {
            ClientDb salesFirstDb = new ClientDb();
           /*this gives us the username of the user currently logged in - WB*/
            var loggedInUser = User.Identity.Name;

            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (employee == null || db.GetHolidaysByEmployee(employee).FirstOrDefault() == null)
            {
                /*If the employee doesn't exist or if there are no holidays against an employee we cannot list any holidays*/
                /*However, if the employee doesn't exist, meaning mapping didn't work - needs some extra validation - WB */

                ViewBag.GeneralHolidays = db.GeneralHolidays.ToList();
                ViewBag.Holidays = null;

                return View();
            
            }

         
            ViewBag.GeneralHolidays = db.GeneralHolidays.ToList();
            ViewBag.Holidays = db.GetHolidaysByEmployee(employee).ToList();
        
            return View();


        }
        public ActionResult FullYearCalendar()
        {
            ClientDb salesFirstDb = new ClientDb();
            /*this gives us the username of the user currently logged in - WB*/
            var loggedInUser = User.Identity.Name;

            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (employee == null || db.GetHolidaysByEmployee(employee).FirstOrDefault() == null)
            {
                /*If the employee doesn't exist or if there are no holidays against an employee we cannot list any holidays*/
                /*However, if the employee doesn't exist, meaning mapping didn't work - needs some extra validation - WB */

                ViewBag.GeneralHolidays = db.GeneralHolidays.ToList();
                ViewBag.Holidays = null;

                return View();

            }


            ViewBag.GeneralHolidays = db.GeneralHolidays.ToList();
            ViewBag.Holidays = db.GetHolidaysByEmployee(employee).ToList();

            return View();


        }
        // GET: /General/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralHoliday generalholiday = db.GeneralHolidays.Find(id);
            if (generalholiday == null)
            {
                return HttpNotFound();
            }
            return View(generalholiday);
        }

        // GET: /General/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /General/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GeneralHolidayId,Name,Description,Type,StartDate,EndDate,Frequency")] GeneralHoliday generalholiday)
        {
            generalholiday.Type = "general";
            if (ModelState.IsValid)
            {
                db.GeneralHolidays.Add(generalholiday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(generalholiday);
        }
        // GET: /General/Create
        public ActionResult CreateIslamic()
        {
            return View();
        }

        // POST: /General/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIslamic([Bind(Include = "GeneralHolidayId,Name,Description,Type,StartDate,EndDate,Frequency")] GeneralHoliday generalholiday)
        {
            generalholiday.Type = "islamic";

                       
            if (ModelState.IsValid)
            {
                db.GeneralHolidays.Add(generalholiday);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(generalholiday);
        }

        // GET: /General/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralHoliday generalholiday = db.GeneralHolidays.Find(id);
            if (generalholiday == null)
            {
                return HttpNotFound();
            }
            return View(generalholiday);
        }

        // POST: /General/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GeneralHolidayId,Name,Description,StartDate,EndDate,Frequency")] GeneralHoliday generalholiday)
        {
            if (ModelState.IsValid)
            {
                db.Entry(generalholiday).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalholiday);
        }

        // GET: /General/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralHoliday generalholiday = db.GeneralHolidays.Find(id);
            if (generalholiday == null)
            {
                return HttpNotFound();
            }
            return View(generalholiday);
        }

        // POST: /General/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GeneralHoliday generalholiday = db.GeneralHolidays.Find(id);
            db.GeneralHolidays.Remove(generalholiday);
            db.SaveChanges();
            return RedirectToAction("Index");
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
