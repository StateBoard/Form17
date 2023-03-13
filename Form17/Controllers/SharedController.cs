using Form17.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Form17.Controllers
{
    public class SharedController : Controller
    {
        Form17Entities db = new Form17Entities();
        // GET: Shared
        public ActionResult Error()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            return View();
        }
        public ActionResult DeActive()
        {
            return View();
        }
    }
}