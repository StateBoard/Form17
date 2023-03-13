using Form17.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Form17.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        Form17Entities db = new Form17Entities();
        public ActionResult NotFound()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            return View();
        }
    }
}