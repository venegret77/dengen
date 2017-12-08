namespace MashZavod.Models
{
    public class NewsModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public NewsModel()
        {

        }

        public NewsModel(string _Title, string _Link, string _PubDate, string _Description, string _Author)
        {
            Title = _Title;
            Link = _Link;
            PubDate = _PubDate;
            Description = _Description;
            Author = _Author;
        }
    }
}