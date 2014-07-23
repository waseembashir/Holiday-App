using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HolidayApp.Core.Model;
using HolidayApp.Core.Data;

namespace HolidayApp.Controllers
{
    public class HolidayController : Controller
    {

        private HolidayAppDb db = new HolidayAppDb();

        // GET: /Holiday/
        public ActionResult Index()
        {
            return View(db.MyHolidays.ToList());
        }
	}
}