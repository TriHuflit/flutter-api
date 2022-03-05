using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Implementations
{
    public class MessageResResponsitory : GenericMongoDB<MessageRes>, IMessageRespository
    {
        public MessageResResponsitory(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
