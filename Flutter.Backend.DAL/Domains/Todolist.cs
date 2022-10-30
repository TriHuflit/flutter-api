using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class Todolist : AuditLogSystem
    {
        public string Title { get; set; }

        public bool status { get; set; }

    }
}
