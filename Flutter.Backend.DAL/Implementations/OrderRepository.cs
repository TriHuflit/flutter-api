using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class OrderRespository : GenericMongoDB<Order> , IOrderRepository
    {
        public OrderRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
