using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class ProductRespository : GenericMongoDB<Product> , IProductRepository
    {
        public ProductRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
