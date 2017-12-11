﻿using System.Collections;

namespace MashZavod.Models
{
    public class NewsModel: IComparer
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

        public int Compare(object x, object y)
        {
            throw new System.NotImplementedException();
        }
    }
}