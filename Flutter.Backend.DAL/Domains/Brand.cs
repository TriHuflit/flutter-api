using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class Brand : AuditLogSystem
    {
        public ObjectId CategoryId { get; set; }

        public string Name { get; set; }

        public string ImageBrand { get; set; } 

        public int IsShow  { get; set; }    

        
    }
}
