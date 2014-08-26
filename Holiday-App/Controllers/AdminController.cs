using System.Linq;
using System.Net;
using System;
using System.Web.Mvc;
using System.Data.Entity;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;
using Microsoft.AspNet.Identity;
using SalesFirst.Core.Data;
using SalesFirst.Core.Service;
using System.Net.Mail;
using System.Collections.Generic;

namespace HolidayApp.Controllers
{

      

    public class AdminController : Controller
    {

        private readonly HolidayAppDb db = new HolidayAppDb();
        private readonly ClientDb salesFirstDb = new ClientDb();
        readonly EmployeeRepository employeeRepository;
        readonly EmployeeService employeeService;

        public AdminController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }

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


        // GET: /Admin/
        public ActionResult Index()
        {
            return View(db.GetPendingHolidays().ToList()); 
        }

        // GET: /Admin/Requests
        public ActionResult Requests()
        {
            return View(db.GetApprovedHolidays().ToList());
        }


        // GET: /Admin/Requests
        public ActionResult RejectedRequests()
        {
            return View(db.GetDisApprovedHolidays().ToList());
        }


        // GET: /Admin/Edit/1
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

        // POST: /Holiday/Edit/1
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HolidayId,StartDate,EndDate,NoOfDays,Employee,BookingDate,HalfDay,Holidaytype,HolidayDescription,Status")] Holiday holiday)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(holiday).State = EntityState.Modified;
               
                db.SaveChanges();
                return RedirectToAction("Requests");
            }
            return View(holiday);
        }


        // GET: /Admin/Create
        [Authorize]
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Personal", Value = "Personal", Selected = true });

            items.Add(new SelectListItem { Text = "Mothly", Value = "Mothly" });

            items.Add(new SelectListItem { Text = "Sick Leave", Value = "Sick Leave" });

            items.Add(new SelectListItem { Text = "Yearly", Value = "Yearly" });

            ViewBag.HolidayType = items;

            var employees = employeeService.GetAll();
            List<SelectListItem> employee_list = new List<SelectListItem>();
            foreach(var name in employees)
            {
                employee_list.Add(new SelectListItem { Text = name.Username, Value = name.Username });
            }
            ViewBag.EmployeeNames = employee_list;
            return View();
        }


        // POST: /Admin/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeNames,HolidayId,StartDate,EndDate,NoOfDays,Employee,BookingDate,BookedBy,Holidaytype,HolidayDescription,HalfDay")] Holiday holiday)
        {
            var loggedInUser = User.Identity.Name;
            var employee = employeeService.GetEmployeeByUsername(Request.Form["EmployeeNames"]);


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
                holiday.BookedBy = "Admin";
                holiday.Status = "Approved";

                db.Holidays.Add(holiday);
                db.SaveChanges();
                //Send email notification to the admin
                ////EmailService email = new EmailService();
                var id = holiday.HolidayId;

                MailMessage message = new MailMessage();
                //  message.To.Add(new MailAddress("zafar.rather@apexure.com"));  //employees email id retrieve from employee table
                message.To.Add(new MailAddress(employee.Username));
                message.From = new MailAddress("zafar.rather@apexure.com");
                // message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
                message.Subject = "New Holiday Booked for you By Admin ";
                message.IsBodyHtml = true;
                var link = "https://localhost:44388/Holiday/Details/" + id;
                message.Body = "This is demo mail Goto to this link <br> <a href='" + link + "'>Click here</a>";
                SmtpClient client = new SmtpClient() { EnableSsl = true };
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential
                ("holiday.apexure", "apexure111");
                client.Send(message);
                //------------------------------------------------------------------------------------
                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        //Get:Admin/Accept/1
        public ActionResult Accept(int id)
        {
            if(id==null)
            {
                return HttpNotFound();
            }
            Holiday holiday = db.Holidays.Find(id);

            if (holiday == null)
            {
                return HttpNotFound();
            }
            holiday.Status = "Approved";
            var date = holiday.StartDate;
            db.Entry(holiday).State = EntityState.Modified;
            db.SaveChanges();
                
            return RedirectToAction("Index");
        }

        //Get:Admin/Reject/1
        public ActionResult Reject(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Holiday holiday = db.Holidays.Find(id);

            if (holiday == null)
            {
                return HttpNotFound();
            }
            holiday.Status = "Rejected";
            db.Holidays.Attach(holiday);
            db.Entry(holiday).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Get: Admin/Details/1

        public ActionResult HolidayDetails(int? id)
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
            ViewData["Startingdate"] = holiday.StartDate;
            return View(holiday);           
        }


	}
} 