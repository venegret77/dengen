using System;
using System.Collections;

namespace MashZavod.Models
{
    public class NewsModel: IComparable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }

        public NewsModel()
        {

        }

        public NewsModel(string _Title, string _Link, string _PubDate, string _Description)
        {
            Title = _Title;
            Link = _Link;
            PubDate = _PubDate;
            Description = _Description;
        }

        public int CompareTo(object obj)
        {
            NewsModel m = (NewsModel)obj;
            return -Convert.ToDateTime(PubDate).CompareTo(Convert.ToDateTime(m.PubDate));
        }
    }
}