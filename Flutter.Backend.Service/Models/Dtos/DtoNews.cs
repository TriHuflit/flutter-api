namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoNews : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public int IsShow { get; set; }
    }
}
