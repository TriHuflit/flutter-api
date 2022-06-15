namespace Flutter.Backend.DAL.Domains
{
    public class News : AuditLogSystem
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Content {get;set;}

        public string Thumbnail { get; set; }

        public int IsShow { get; set; }
    }
        
}
