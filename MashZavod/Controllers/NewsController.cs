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
            //Технологии
            XDocument TechnXML = XDocument.Load("https://news.yandex.ru/computers.rss");
            var Techn = from feed in TechnXML.Descendants("item")
                        select new
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = feed.Element("description").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            //Наука
            XDocument ScienceXML = XDocument.Load("https://news.yandex.ru/science.rss");
            var Science = from feed in ScienceXML.Descendants("item")
                        select new
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = feed.Element("description").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            //Владимирская область
            XDocument VladimirXML = XDocument.Load("https://news.yandex.ru/Vladimir/index.rss");
            var Vladimir = from feed in VladimirXML.Descendants("item")
                        select new
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = feed.Element("description").Value,
                            PubDate = feed.Element("pubDate").Value
                        };
            //Политика
            XDocument PoliticXML = XDocument.Load("https://news.yandex.ru/politics.rss");
            var Politic = from feed in PoliticXML.Descendants("item")
                           select new
                           {
                               Title = feed.Element("title").Value,
                               Link = feed.Element("link").Value,
                               Description = feed.Element("description").Value,
                               PubDate = feed.Element("pubDate").Value
                           };
            //Заносим все в единый массив
            //Возвращаем
            return null;
        }
    }
}