using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class MessageResResponsitory : GenericMongoDB<MessageRes>, IMessageRepository
    {
        public MessageResResponsitory(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
