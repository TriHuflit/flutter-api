using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class CaterogyRespository : GenericMongoDB<Category> , ICategoryRepository
    {
        public CaterogyRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
