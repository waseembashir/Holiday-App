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
using SalesFirst.Core.Service;
using SalesFirst.Core.Data;

namespace HolidayApp.Controllers
{
    [Authorize]
    public class EmployeeQuotaController : Controller
    {
        private HolidayAppDb db = new HolidayAppDb();

        readonly EmployeeRepository employeeRepository;
       readonly EmployeeService employeeService;

       public EmployeeQuotaController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }

        // GET: /EmployeeQuota/
        public ActionResult Manage()
        {
            return View(db.EmployeeQuotas.ToList());
        }

        // GET: /EmployeeQuota/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeQuota employeequota = db.EmployeeQuotas.Find(id);
            if (employeequota == null)
            {
                return HttpNotFound();
            }
            return View(employeequota);
        }
        public ActionResult Index()
        {

            /*this gives us the username of the user currently logged in - WB*/
            var loggedInUser = User.Identity.Name;

            var employee = employeeService.GetEmployeeByUsername(loggedInUser);
            if (employee == null || db.GetEmployeeQuotaByEmployee(employee) == null)
            {
                /*If the employee doesn't exist or if there are no quotas against an employee we cannot list any quotas*/
                /*However, if the employee doesn't exist, meaning mapping didn't work - needs some extra validation - NN */
                ViewBag.Message = "No Quota Information Found";
                return View();
            }

            return View(db.GetEmployeeQuotaByEmployee(employee));

           
        }

        // GET: /EmployeeQuota/Create
        public ActionResult Create()
        {
            ViewBag.Employees = new SelectList(employeeService.GetAll(), "EmployeeId", "Username");
            return View();
        }

        // POST: /EmployeeQuota/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeQuotaId,EmployeeId,PaidQuota,NonPaidQuota")] EmployeeQuota employeequota)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeQuotas.Add(employeequota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeequota);
        }

        // GET: /EmployeeQuota/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Employees = new SelectList(employeeService.GetAll(), "EmployeeId", "Username");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeQuota employeequota = db.EmployeeQuotas.Find(id);
            if (employeequota == null)
            {
                return HttpNotFound();
            }
            
            return View(employeequota);
        }

        // POST: /EmployeeQuota/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeQuotaId,EmployeeId,PaidQuota,NonPaidQuota")] EmployeeQuota employeequota)
        {
            ViewBag.Employees = new SelectList(employeeService.GetAll(), "EmployeeId", "Username");
            if (ModelState.IsValid)
            {
                db.Entry(employeequota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeequota);
        }

        // GET: /EmployeeQuota/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeQuota employeequota = db.EmployeeQuotas.Find(id);
            if (employeequota == null)
            {
                return HttpNotFound();
            }
            return View(employeequota);
        }

        // POST: /EmployeeQuota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeQuota employeequota = db.EmployeeQuotas.Find(id);
            db.EmployeeQuotas.Remove(employeequota);
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
