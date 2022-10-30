using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using MongoDB.Driver;

namespace Flutter.Backend.DAL.Implementations
{
    public class TodoListRepository : GenericMongoDB<Todolist>, ITodoListRepository
    {
        public TodoListRepository(IMongoClient dbClient) : base(dbClient)
        {

        }
    }
}
