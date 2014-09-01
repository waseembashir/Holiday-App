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

namespace HolidayApp.Controllers
{
    public class HolidayDescriptionController : Controller
    {
        private HolidayAppDb db = new HolidayAppDb();

        // GET: /HolidayDescription/
        public ActionResult Index()
        {
            return View(db.HolidayDescriptions.ToList());
        }

        // GET: /HolidayDescription/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayDescription holidaydescription = db.HolidayDescriptions.Find(id);
            if (holidaydescription == null)
            {
                return HttpNotFound();
            }
            return View(holidaydescription);
        }

        // GET: /HolidayDescription/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /HolidayDescription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="HolidayDescriptionId,HolidayType,TypeFor,HolidayColor")] HolidayDescription holidaydescription)
        {
            if (ModelState.IsValid)
            {
                holidaydescription.HolidayColor = "#"+holidaydescription.HolidayColor;
                db.HolidayDescriptions.Add(holidaydescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holidaydescription);
        }

        // GET: /HolidayDescription/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayDescription holidaydescription = db.HolidayDescriptions.Find(id);
            if (holidaydescription == null)
            {
                return HttpNotFound();
            }
            return View(holidaydescription);
        }

        // POST: /HolidayDescription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HolidayDescriptionId,HolidayType,TypeFor,HolidayColor")] HolidayDescription holidaydescription)
        {
            if (ModelState.IsValid)
            {
                holidaydescription.HolidayColor = "#" + holidaydescription.HolidayColor;
                db.Entry(holidaydescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holidaydescription);
        }

        // GET: /HolidayDescription/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayDescription holidaydescription = db.HolidayDescriptions.Find(id);
            if (holidaydescription == null)
            {
                return HttpNotFound();
            }
            return View(holidaydescription);
        }

        // POST: /HolidayDescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HolidayDescription holidaydescription = db.HolidayDescriptions.Find(id);
            db.HolidayDescriptions.Remove(holidaydescription);
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
