using Flutter.Backend.DAL.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Flutter.Backend.DAL.Implementations
{
    public class GenericMongoDB<T> : IResposibity<T>
    {
        protected IMongoClient _dbClient;
        private readonly IMongoCollection<T> _mongoCollection;

        public GenericMongoDB(IMongoClient dbClient)
        {     
            var database = dbClient.GetDatabase("Flutter-Project");
            var collection = database.GetCollection<T>(typeof(T).Name);

            _mongoCollection = collection;
        }

        public void Add(T item)
        {
            _mongoCollection.InsertOne(item);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> specification)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(string Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        //protected virtual IMongoCollection<T> GetDbCollection()
        //{

            
        //    var database = _dbClient.GetDatabase("Flutter-Project");
        //    var collection = database.GetCollection<T>(typeof(T).Name);

        //    return collection;
        //}
    }
}
