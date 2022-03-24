using MongoDB.Bson;

namespace Flutter.Backend.DAL.Domains
{
    public class TemplateSendMail
    {
        public ObjectId Id { get; set; }

        public string Key { get; set; }

        public string TemplateHTML { get; set; }
    }
}
