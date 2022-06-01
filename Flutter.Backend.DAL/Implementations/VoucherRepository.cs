using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class VoucherRespository : GenericMongoDB<Voucher> , IVoucherRepository
    {
        public VoucherRespository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
