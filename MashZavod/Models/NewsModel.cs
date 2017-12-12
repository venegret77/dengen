using System;
using System.Collections;
using System.Collections.Generic;

namespace MashZavod.Models
{
    public class NewsModel: IComparable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
        public int Relevance { get; set; }

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

        public NewsModel(string _Title, string _Link, string _PubDate, string _Description, List<string> tags)
        {
            Title = _Title;
            Link = _Link;
            PubDate = _PubDate;
            Description = _Description;
            Relevance = CalcRelevance(tags);
        }

        public int CalcRelevance(List<string> tags)
        {
            string _Description = Description.ToLower();
            int i = 0;
            foreach (var tag in tags)
                if (_Description.Contains(tag.ToLower()))
                    i++;
            return i;
        }

        public int CompareTo(object obj)
        {
            NewsModel m = (NewsModel)obj;
            return -Relevance.CompareTo(m.Relevance);
        }
    }
}