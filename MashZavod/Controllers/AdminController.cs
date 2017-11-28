using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MashZavod.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AdminIndex()
        {
            if (User.Identity.Name != "Admin")
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}