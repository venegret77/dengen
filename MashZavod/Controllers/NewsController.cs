using MashZavod.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml.Linq;
using LingvoNET;
using MashZavod.Models.DbModels;

namespace MashZavod.Controllers
{
    public class NewsController : Controller
    {
        private List<string> tags = new List<string>();
        private List<string> links = new List<string>();

        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News()
        {
            links.Add("https://lenta.ru/rss/news/russia/");
            tags.Add("Наука");
            tags.Add("Производства");
            tags.Add("производство");
            tags.Add("Техника");
            tags.Add("Изобретение");
            tags.Add("Науки");
            tags.Add("IT");
            tags.Add("Техники");
            List<NewsModel> NewsList = new List<NewsModel>();
            foreach (var link in links)
            {
                XDocument newsXML = XDocument.Load(link);
                var news = from feed in newsXML.Descendants("item")
                           select new
                           {
                               Title = feed.Element("title").Value,
                               Link = feed.Element("link").Value,
                               Description = feed.Element("description").Value,
                               PubDate = feed.Element("pubDate").Value
                           };
                foreach (var _news in news)
                {
                    NewsList.Add(new NewsModel(_news.Title, _news.Link, _news.PubDate, _news.Description));
                }
            }
            NewsList.Sort();
            ViewBag.listNews = NewsList.Distinct();
            return View();
        }

        private List<string> FindNoun(string _noun)
        {
            List<string> nouns = new List<string>();
            var noun = Nouns.FindOne(_noun);
            if (noun != null)
            {
                nouns.Add(noun[Case.Accusative, Number.Plural]);
                nouns.Add(noun[Case.Accusative, Number.Singular]);
                nouns.Add(noun[Case.Dative, Number.Plural]);
                nouns.Add(noun[Case.Dative, Number.Singular]);
                nouns.Add(noun[Case.Genitive, Number.Plural]);
                nouns.Add(noun[Case.Genitive, Number.Singular]);
                nouns.Add(noun[Case.Instrumental, Number.Plural]);
                nouns.Add(noun[Case.Instrumental, Number.Singular]);
                nouns.Add(noun[Case.Locative, Number.Plural]);
                nouns.Add(noun[Case.Locative, Number.Singular]);
                nouns.Add(noun[Case.Nominative, Number.Plural]);
                nouns.Add(noun[Case.Nominative, Number.Singular]);
            }
            return nouns;
        }

        [HttpPost]
        public ActionResult Tags(TagsNews model)
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
               var teg = db.TagsNews.Add(new TagsNews()
                {
                    Tag = model.Tag
                });

                db.SaveChanges();

          
                // Проверяем, что пользователь успешно добавился
                teg = db.TagsNews.Where(u => u.Tag == model.Tag).FirstOrDefault();
                if ( teg != null)
                {
                    return RedirectToAction("AdminIndex", "Admin");

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RSS(SourcesRSS model)
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                db.SourcesRSS.Add(new SourcesRSS()
                {
                    Link = model.Link
                });
                db.SaveChanges();
            }
            return View();
        }
    }
}