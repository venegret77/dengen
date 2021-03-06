﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MashZavod.Models
{
    public class NewsModel: IComparable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Link { get; set; }
        public string Source { get; set; }
        public int Relevance { get; set; }

        public NewsModel()
        {

        }

        public NewsModel(string _Title, string _Link, string _PubDate, string _Description, string _Source, List<string> tags)
        {
            Title = _Title;
            Link = _Link;
            PubDate = Convert.ToDateTime(_PubDate);
            Description = _Description;
            Source = _Source;
            Relevance = CalcRelevance(tags);
        }

        public int CalcRelevance(List<string> tags)
        {
            int i = 0;
            string _tag, _Description, _Title;
            foreach (var tag in tags)
            {
                _Description = " " + Description.ToLower() + " ";
                _Title = " " + Title.ToLower() + " ";
                _tag = " " + tag.ToLower() + " ";
                if (_Description.Contains(_tag))
                {
                    i++;
                }
                if (_Title.Contains(_tag))
                {
                    i++;
                }
            }
            return i;
        }

        public int CompareTo(object obj)
        {
            NewsModel m = (NewsModel)obj;
            return -Relevance.CompareTo(m.Relevance);
        }
    }
}