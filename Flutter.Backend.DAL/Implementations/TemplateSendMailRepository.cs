using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class TemplateSendMailRepository : GenericMongoDB<TemplateSendMail>, ITemplateSendMailRepository
    {
        public TemplateSendMailRepository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
