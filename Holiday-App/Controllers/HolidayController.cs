using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using SalesFirst.Core.Service;


namespace HolidayApp.Controllers
{
    
    public class HolidayController : Controller
    {

       private HolidayAppDb db = new HolidayAppDb();
        // GET: /Holiday/
        
        // [Authorize] should use this helper attribute. This will force the user to login before they can 
        // View or book holidays. - WB
       /// <summary>
       /// Indexes this instance.
       /// </summary>
       /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            /*this gives us the username of the user currently logged in - WB*/
            var loggedInUser = User.Identity.Name;

            /*Concatenating the word @apexure.com to the username to map to the employee username. -WB*/

            var employee = db.GetEmployeeByUsername(loggedInUser+"@apexure.com");
            if (employee == null || db.GetHolidaysByEmployee(employee).FirstOrDefault()==null)
            {
                /*If the employee doesn't exist or if there are no holidays against an employee we cannot list any holidays*/
                /*However, if the employee doesn't exist, meaning mapping didn't work - needs some extra validation - WB */
                return RedirectToAction("Create");
            }


            return View(db.GetHolidaysByEmployee(employee).ToList());


        }

        // GET: /Holiday/Create
         [Authorize]
        public ActionResult Create()
        {
            var memberId = User.Identity.GetUserId();
            ViewData["Message"] = memberId;

            return View();
        }

        // POST: /Holiday/Create

         [HttpPost]
         [ValidateAntiForgeryToken]
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

        // GET: /Holidays/Delete/5
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
        public ActionResult Edit([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = db.GetEmployeeByUsername(loggedInUser);
            if (ModelState.IsValid)
            {
                holiday.Employee = employee;
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