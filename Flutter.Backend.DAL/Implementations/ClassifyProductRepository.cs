using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class ClassifyProductRepository : GenericMongoDB<ClassifyProduct>, IClassifyProductRepository
    {
        public ClassifyProductRepository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
