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
using SalesFirst.Core.Model;
using SalesFirst.Core.Data;
using SalesFirst.Core.Service;

namespace HolidayApp.Controllers
{
    public class EmployeeController : Controller
    {
        private ClientDb db = new ClientDb();
        readonly EmployeeRepository employeeRepository;
        readonly EmployeeService employeeService;

        public EmployeeController()
        {
            employeeRepository = new EmployeeRepository(db);
            employeeService = new EmployeeService(employeeRepository);
        }

        // GET: /Employee/
        public ActionResult Index()
        {
            return View(employeeService.GetAll());
        }

        // GET: /Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = employeeService.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);

            }

        // GET: /Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EmployeeId,FirstName,LastName,BirthDate,HireDate,TerminationDate,Address,ContactNumber,PersonalEmail,Username,JobTitle")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeService.Create(employee);
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: /Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = employeeService.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: /Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EmployeeId,FirstName,LastName,BirthDate,HireDate,TerminationDate,Address,ContactNumber,PersonalEmail,Username,JobTitle")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeService.Update(employee);
                //db.Entry(employee).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: /Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = employeeService.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = employeeService.Get(id);
            employeeService.Delete(employee.EmployeeId);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //employeeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
