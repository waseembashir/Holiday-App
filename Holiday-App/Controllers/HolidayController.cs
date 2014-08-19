using System.Linq;
using System.Net;
using System;
using System.Web.Mvc;
using System.Data.Entity;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
<<<<<<< HEAD
using System.Net.Mail;
using SalesFirst.Core.Service;
using System.Collections.Generic;
=======
using SalesFirst.Core.Service;
>>>>>>> origin/master


namespace HolidayApp.Controllers
{
    
    public class HolidayController : Controller
    {

       private readonly HolidayAppDb db = new HolidayAppDb();
       private readonly ClientDb salesFirstDb = new ClientDb();
<<<<<<< HEAD

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
    
=======
       readonly EmployeeRepository employeeRepository;
       readonly EmployeeService employeeService;

        public HolidayController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }

>>>>>>> origin/master
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
         [Authorize]
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Personal", Value = "Personal" });

            items.Add(new SelectListItem { Text = "Mothly", Value = "Mothly" });

            items.Add(new SelectListItem { Text = "Sick Leave", Value = "Sick Leave", Selected = true });

            items.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });

            ViewBag.HolidayType = items;

            return View();
        }

        
        // POST: /Holiday/Create

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee,BookingDate,BookedBy,Holidaytype,HolidayDescription")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = employeeService.GetEmployeeByUsername(loggedInUser);

<<<<<<< HEAD

            
=======
            // Can i  use below like structur to show errors before saving data to db
            // if(holiday.NoOfDays<2)
            // {
            //   ModelState.AddModelError("NoOfDays", "Days greater than 1 please");
            //    return RedirectToAction("Create");
            // }
>>>>>>> origin/master

            if (ModelState.IsValid)
            {
                holiday.Employee = employee;
                holiday.BookingDate = DateTime.Today;
                holiday.BookedBy = employee.FirstName;

                db.Holidays.Add(holiday);
                db.SaveChanges();
                ////Send email notification to the admin
                //    //EmailService email = new EmailService();
                //    var id = holiday.HolidayId;
                   
                //    MailMessage message = new MailMessage();
                //  //  message.To.Add(new MailAddress("zafar.rather@apexure.com"));  //employees email id retrieve from employee table
                //    message.To.Add(new MailAddress("waseem@apexure.com"));
                //    message.From = new MailAddress("zafar.rather@apexure.com");
                //    // message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
                //    message.Subject = "New Holiday Booking Request By "+employee.FirstName;
                //    message.IsBodyHtml = true;
                //    var link = "https://localhost:44388/Holiday/Edit/" + id;
                //    message.Body = "This is demo mail Goto to this link <br> <a href='"+link +"'>Click here</a>";
                //    SmtpClient client = new SmtpClient() {EnableSsl= true };
                //    client.Host = "smtp.gmail.com";
                //    client.Port = 587;
                //    client.UseDefaultCredentials = false;
                //    client.Credentials = new System.Net.NetworkCredential
                //    ("holiday.apexure", "apexure111");
                //    client.Send(message);
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
            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (ModelState.IsValid)
            {
              //  holiday.Employee = employee;
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