using System.Linq;
using System.Net;
using System;
using System.Web.Mvc;
using System.Data.Entity;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
using System.Net.Mail;
using SalesFirst.Core.Service;
using System.Collections.Generic;




namespace HolidayApp.Controllers
{
    /// <summary>
    /// Base controller for Employee For Holiday based requests
    /// </summary>
    [Authorize]
    public class HolidayController : Controller
    {

       private readonly HolidayAppDb db = new HolidayAppDb();
       private readonly ClientDb salesFirstDb = new ClientDb();


        //-------------Make the list of holiday type-------------------------
       public enum holidaytypes { personal, Monthly, Sick_Leave, Yearly };

       private void SetViewBagHolidayType(holidaytypes selectedType)
       {

           IEnumerable<holidaytypes> values =

                             Enum.GetValues(typeof(holidaytypes))

                             .Cast<holidaytypes>();

           IEnumerable<SelectListItem> items =

               from value in values

               select new SelectListItem

               {

                   Text = value.ToString(),

                   Value = value.ToString(),

                   Selected = value == selectedType,

               };



           ViewBag.HolidayType = items;

       }

        //-------------------------------------------------------------------
    

       readonly EmployeeRepository employeeRepository;
       readonly EmployeeService employeeService;

        public HolidayController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }


        // GET: /Holiday/
        
        // [Authorize] should use this helper attribute. This will force the user to login before they can 
        // View or book holidays. - WB
       /// <summary>
       /// Indexes this instance.
       /// </summary>
       /// <returns></returns>
        
        public ActionResult Index()
        {
            /*this gives us the username of the user currently logged in - WB*/
            var loggedInUser = User.Identity.Name;

            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (employee == null || db.GetHolidaysByEmployee(employee).FirstOrDefault()==null)
            {
                /*If the employee doesn't exist or if there are no holidays against an employee we cannot list any holidays*/
                /*However, if the employee doesn't exist, meaning mapping didn't work - needs some extra validation - WB */
                ViewBag.Message = "No Holidays Booked Yet";
                return View();
            }

           

            return View(db.GetHolidaysByEmployee(employee).ToList());


        }

        // GET: /Holiday/Create
     
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Personal", Value = "Personal", Selected = true });

            items.Add(new SelectListItem { Text = "Mothly", Value = "Mothly" });

            items.Add(new SelectListItem { Text = "Sick Leave", Value = "Sick Leave"});

            items.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });

            ViewBag.HolidayTypes = new SelectList(db.GetAllEmployeeHolidayDescriptions, "HolidayType", "HolidayType");

            ViewBag.HolidayType = items;

            return View();
        }

        
        // POST: /Holiday/Create

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee,BookingDate,BookedBy,Holidaytype,HolidayDescription,HalfDay")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = employeeService.GetEmployeeByUsername(loggedInUser);


            // Can i  use below like structur to show errors before saving data to db
            // if(holiday.NoOfDays<2)
            // {
            //   ModelState.AddModelError("NoOfDays", "Days greater than 1 please");
            //    return RedirectToAction("Create");
            // }


            if (ModelState.IsValid)
            {
                holiday.Employee = employee;
                holiday.BookingDate = DateTime.Today;
                holiday.BookedBy = employee.Username;

                db.Holidays.Add(holiday);
                db.SaveChanges();
                //Send email notification to the admin
                EmailService email = new EmailService();
                string email_body = "New Holiday Request from "+employee.Username+". Click the link below for details<br/>";
                email_body = email_body + "<a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "/Admin/HolidayDetails/" + holiday.HolidayId + "'>Check Details</a>";
                List<string> emailcc = new List<string>();
                email.SendEmail("New Holiday Request", email_body,"zafar.rather@apexure.com", emailcc);

                //------------------------------------------------------------------------------------
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
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }

            if (id == null || holiday.Status !=null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            Holiday holiday = db.Holidays.Find(id);
            if (holiday == null)
            {
                return HttpNotFound();
            }

            if (id == null || holiday.Status!=null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var loggedInUser = User.Identity.Name;
            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            ViewBag.EmployeeId = employee.EmployeeId;

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Personal", Value = "Personal", Selected = true });

            items.Add(new SelectListItem { Text = "Mothly", Value = "Mothly" });

            items.Add(new SelectListItem { Text = "Sick Leave", Value = "Sick Leave" });

            items.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });

            ViewBag.HolidayType = items;

            return View(holiday);

        }

        // POST: /Holiday/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee,HalfDay,Holidaytype,HolidayDescription")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (ModelState.IsValid)
            {
               holiday.Employee = employee;
                db.Entry(holiday).State = EntityState.Modified;
                holiday.BookingDate = DateTime.Today;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holiday);
        }

        /// <summary>
        /// Generate Reports 
        /// </summary>
        /// <returns></returns>
        // GEt: /Holiday/Report
        public ActionResult Report(int id)
        {
            return View();
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