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
                

                foreach (var el in req_message)
                {
                    var y = db.users.FirstOrDefault(u=>u.id_users==el.id_users);
                    var x = "";
                    if (y != null)
                    {
                        x = y.Login;
                    }
                    ListMessage.Add(new BigChatModels { LoginUs = x,
                        Text_message = el.text_message,
                        Time_message = (DateTime)el.datetime.Value.Date }); //вопрос с привидением типов у даты (DateTime)el.datetime
                } 
            }
            ViewBag.Chat = ListMessage;


            return View("BigChat");
        }

        [HttpPost]
        public ActionResult Index(message model)//функция отправки соощения
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                model.datetime = DateTime.Now;
                model.id_users = db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;

                //добовляем в бд
                db.message.Add(model);

                db.SaveChanges();
                return RedirectToAction("Index", "BigChat");///поправить путь
            }
        }
    }
}