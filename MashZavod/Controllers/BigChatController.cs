using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MashZavod.Models.DbModels;
using MashZavod.Models;

namespace MashZavod.Controllers
{
    public class BigChatController : Controller
    {
        // GET: BigChat
        [HttpGet]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            List<BigChatModels> ListMessage = new List<BigChatModels>();
            /*Возвращает историю сообщений*/
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                var zaprosik = db.message;


                foreach (var el in zaprosik)
                {
                    ListMessage.Add(new BigChatModels { Login = el.id_users.ToString(), Message = el.text_message, Time = el.datetime });
                }//Login = db.users.Where(u => u.id_users == el.LoginUser)
            }
            ViewBag.Chat = ListMessage;


            return PartialView("BigChat");
        }

        [HttpPost]
        public void Index(message model)//функция отправки соощения
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                db.message.Add(model);
                db.SaveChanges();
            }
        }
    }
}