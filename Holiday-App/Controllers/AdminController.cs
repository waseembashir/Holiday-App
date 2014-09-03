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


    /// <summary>
    /// Base controller for all Admin area
    /// </summary>
    [Authorize]
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
        public ActionResult Create([Bind(Include = "EmployeeNames,StartDate,EndDate,NoOfDays,Holidaytype,HolidayDescription,HalfDay")] Holiday holiday)
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
                holiday.BookedBy = employee.Username;
                holiday.Status = "Approved";

                db.Holidays.Add(holiday);
                db.SaveChanges();
                //Send email notification to the admin
                EmailService email = new EmailService();
                string email_body = "Holiday has been booked for you by Admin. Click the link below for details<br/>";
                email_body = email_body + "<a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "/Holiday/Details/" + holiday.HolidayId + "'>Check Details</a>";
                List<string> emailcc = new List<string>();
                email.SendEmail("Holiday Has been booked for you by Admin",email_body,employee.Username,emailcc);
               
                //------------------------------------------------------------------------------------
                return RedirectToAction("Index");
            }

            return View(holiday);
        }

        //Get:Admin/Accept/1
        public ActionResult Accept(int? id)
        {
            if(id==null)
            {
                return HttpNotFound();
            }
            Holiday holiday = db.Holidays.Find(id);

            if (holiday == null || holiday.StartDate < DateTime.Today)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            holiday.Status = "Approved";
            var date = holiday.StartDate;
            db.Entry(holiday).State = EntityState.Modified;
            db.SaveChanges();
            //Send email notification to the admin
            EmailService email = new EmailService();
            string email_body = "Congratulations your Holiday Request has been approved. Click the link below for details<br/>";
            email_body = email_body + "<a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "/Holiday/Details/" + holiday.HolidayId + "'>Check Details</a>";
            List<string> emailcc = new List<string>();
            email.SendEmail("Holiday Has been booked for you by Admin", email_body, holiday.Employee.Username, emailcc);

            //------------------------------------------------------------------------------------
                
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

            if (holiday == null || holiday.StartDate <= DateTime.Today)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            holiday.Status = "Rejected";
            db.Holidays.Attach(holiday);
            db.Entry(holiday).State = EntityState.Modified;
            db.SaveChanges();
            //Send email notification to the admin
            EmailService email = new EmailService();
            string email_body = "Your Holiday Request has been Rejected. Click the link below for details<br/>";
            email_body = email_body + "<a href='" + Request.Url.GetLeftPart(UriPartial.Authority) + "/Holiday/Details/" + holiday.HolidayId + "'>Check Details</a>";
            List<string> emailcc = new List<string>();
            email.SendEmail("Your Holiday request has been rejected Admin", email_body, holiday.Employee.Username, emailcc);

            //------------------------------------------------------------------------------------
                
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