using MongoDB.Bson;
using System.Collections.Generic;

namespace Flutter.Backend.DAL.Domains
{
    public class FeedBack : AuditLogSystem
    {
        public ObjectId UserId { get; set; }

        public ObjectId ProductId { get; set; }

        public string Content { get; set; }

        public int Star { get; set; }

        public List<string> ImageFeedBack { get; set; }
    }
}
