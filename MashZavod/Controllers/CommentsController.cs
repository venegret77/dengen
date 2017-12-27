using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MashZavod.Models.DbModels;
using MashZavod.Models;

namespace MashZavod.Controllers
{
    public class CommentsController : Controller
    {
        [HttpGet]
        public ActionResult Index(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            ViewBag.Id = id;
            return View("Commets");
        }

        [HttpGet]
        public ActionResult GP(int id)
        {
            List<CommentsModels> ListMessage = new List<CommentsModels>();
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                List<Comments> listComments = db.Comments.Where(u => u.id_doc == id).ToList();
                foreach(var elem in listComments)
                {
                    ListMessage.Add(new CommentsModels
                    {
                        LoginUs = db.users.FirstOrDefault(u => u.id_users == elem.id_user).Login,
                        Text_comments = elem.text_comm,
                        Time_comments = (DateTime)elem.date
                    });
                }
            }
            ViewBag.Id = id;
            ViewBag.Comments = ListMessage;
            return PartialView("CommPartial");
        }

        //Post: comments
        [HttpPost]
        public ActionResult Indexw(Comments model)
        {
            List<CommentsModels> ListMessage = new List<CommentsModels>();
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                model.date = DateTime.Now;
                model.id_user = db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
                db.Comments.Add(model);
                db.SaveChanges();

                List<Comments> listComments = db.Comments.Where(u => u.id_doc == model.id_doc).ToList();
                foreach (var elem in listComments)
                {
                    ListMessage.Add(new CommentsModels
                    {
                        LoginUs = db.users.FirstOrDefault(u => u.id_users == elem.id_user).Login,
                        Text_comments = elem.text_comm,
                        Time_comments = (DateTime)elem.date
                    });
                }
            }
            ViewBag.Comments = ListMessage;
            return PartialView("CommPartial");
        }
    }
}