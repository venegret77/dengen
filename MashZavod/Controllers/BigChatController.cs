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
                var req_message = db.message;
                var req_chat = db.chat;

                foreach (var el in req_message)
                {
                    ListMessage.Add(new BigChatModels { Login = el.id_users.ToString(),
                        Text_message = el.text_message,
                        Time_message = (DateTime)el.datetime }); //вопрос с привидением типов у даты (DateTime)el.datetime
                } 
            }
            ViewBag.Chat = ListMessage;


            return PartialView("BigChat");
        }

        [HttpPost]
        public void Index(message model)//функция отправки соощения
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {               
                //добовляем в бд
                db.message.Add(model);
                db.SaveChanges();
                return;
            }
        }
    }
}