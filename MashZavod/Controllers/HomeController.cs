using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MashZavod.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        string res;
        public string Get(int id)
        {
           // database_murom_factoryEntities A = new database_murom_factoryEntities();
            //if (A.users.First().Login == "admin")
                // res = "ok";
            return "dfgfdgdfg";
        }
       
    }
}
