using MashZavod.Models;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml.Linq;

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
            XDocument feedXML = XDocument.Load("https://news.rambler.ru/rss/tech/");

            var feeds = from feed in feedXML.Descendants("item")
                        select new
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = feed.Element("description").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            return null;
        }
    }
}