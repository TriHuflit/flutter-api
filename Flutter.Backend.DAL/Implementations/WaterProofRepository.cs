using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class WaterProofRepository : GenericMongoDB<WaterProof> , IWaterProofRepository
    {
        public WaterProofRepository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
