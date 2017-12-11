using MashZavod.Models;
using System.Collections.Generic;
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

        public ActionResult News()
        {
            List<NewsModel> NewsList = new List<NewsModel>();
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
            foreach (var techn in Techn)
            {
                NewsList.Add(new NewsModel(techn.Title, techn.Link, techn.PubDate, techn.Description));
            }
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
            foreach (var cience in Science)
            {
                NewsList.Add(new NewsModel(cience.Title, cience.Link, cience.PubDate, cience.Description));
            }
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
            foreach (var vladimir in Vladimir)
            {
                NewsList.Add(new NewsModel(vladimir.Title, vladimir.Link, vladimir.PubDate, vladimir.Description));
            }
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
            foreach (var politic in Politic)
            {
                NewsList.Add(new NewsModel(politic.Title, politic.Link, politic.PubDate, politic.Description));
            }
            NewsList.Sort();
            ViewBag.listNews = NewsList;
            //Заносим все в единый массив
            //Возвращаем
            return View();
        }
    }
}