using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class FeedBackRepository : GenericMongoDB<FeedBack>, IFeedBackRepository
    {
        public FeedBackRepository(IMongoClient dbClient):base(dbClient)
        {

        }
    }
}
