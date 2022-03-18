
namespace Flutter.Backend.DAL.Domains
{
    public class Category : AuditLogSystem
    {
        public string Name { get; set; }

        public string ImageCategory { get; set; }

        public int IsShow { get; set; }
    }
}
