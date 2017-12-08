using MashZavod.Models;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace MashZavod.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNews()
        {
            //Объект запроса
            HttpWebRequest rew = (HttpWebRequest)WebRequest.Create("https://news.rambler.ru/rss/tech/");
            // Отправить запрос и получить ответ
            HttpWebResponse resp = (HttpWebResponse)rew.GetResponse();
            // Получить поток
            Stream str = resp.GetResponseStream();
            return null;
        }
    }
}