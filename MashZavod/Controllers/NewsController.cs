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
        private List<string> links = new List<string>();

        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        private List<string> GetTags()
        {
            List<string> tags = new List<string>();
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                foreach (var tag in db.TagsNews)
                {
                    var _tags = FindNoun(tag.Tag);
                    if (_tags.Count == 0)
                    {
                        tags.Add(tag.Tag);
                    }
                    else
                    {
                        foreach (var _tag in _tags)
                        {
                            tags.Add(_tag);
                        }
                    }
                }
            }
            return tags;
        }

        public ActionResult News()
        {
            links.Add("https://lenta.ru/rss/news/russia/");
            
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
                db.TagsNews.Add(new TagsNews()
                {
                    Tag = model.Tag
                });
                db.SaveChanges();
            }
            return RedirectToAction("Tags", "News");
        }

        [HttpGet]
        public ActionResult TagsDel(int id)
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                db.TagsNews.Remove(db.TagsNews.Where(u => u.ID == id).FirstOrDefault());
                db.SaveChanges();
            }
             
            return RedirectToAction("Tags", "News");
        }

        public ActionResult Tags()
        {
            List<TagsNews> tags = new List<TagsNews>();
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                foreach (var tag in db.TagsNews)
                {
                    tags.Add(tag);
                }
            }
            ViewBag.Tags = tags;
            return View();
        }

        private bool CheckLinkRSS(string Link)
        {
            //Проверка RSS ссылки на корректность
            return false;
        }

        [HttpPost]
        public ActionResult RSS(SourcesRSS model)
        {
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                db.SourcesRSS.Add(new SourcesRSS()
                {
                    Link = model.Link,
                    Description = model.Description
                });
                db.SaveChanges();
            }
            return RedirectToAction("RSS", "News");
        }

        public ActionResult RSS()
        {
            List<SourcesRSS> _rss = new List<SourcesRSS>();
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                foreach (var rss in db.SourcesRSS)
                {
                    _rss.Add(rss);
                }
            }
            ViewBag.Tags = _rss;
            return View();
        }
    }
}