using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class NewsRepository : GenericMongoDB<News>,INewsRepository
    {
        public NewsRepository(IMongoClient dbClient) :base (dbClient)
        {

        }
    }
}
