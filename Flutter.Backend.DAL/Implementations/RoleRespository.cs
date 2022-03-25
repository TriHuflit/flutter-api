using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class RoleRespository : GenericMongoDB<Role>, IRoleRepository
    {
        public RoleRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
