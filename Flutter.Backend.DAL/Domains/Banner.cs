using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class Banner : AuditLogSystem
    {
        public string ImageBanner { get; set; }
        
        public ObjectId IdProduct { get; set; }

        public int IsShowOnMobile { get; set; }

        public string Content { get; set; }


        
    }
}
