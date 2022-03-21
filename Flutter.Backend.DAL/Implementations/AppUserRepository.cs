using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class AppUserRepository : GenericMongoDB<AppUser>, IAppUserRepository
    {
        public AppUserRepository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
