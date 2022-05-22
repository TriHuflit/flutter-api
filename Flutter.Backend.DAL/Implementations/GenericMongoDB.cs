using Flutter.Backend.DAL.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Flutter.Backend.DAL.Implementations
{
    public class GenericMongoDB<T> : IReposibity<T>
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


        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification)
        {
            var data = await _mongoCollection.FindAsync(specification);
            return data.ToList();
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> specification)
        {
            var data = await _mongoCollection.FindAsync(specification);
            return data.FirstOrDefault();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            var data = await _mongoCollection.FindAsync(_ => true);
            return data.ToList();
        }

        public void Update( T item , Expression<Func<T, bool>> specification)
        {
            var properties = item.GetType().GetProperties();
            UpdateDefinition<T> definition = null;
            UpdateDefinitionBuilder<T> builder = new UpdateDefinitionBuilder<T>();
            foreach (var property in properties)
            {
                if (definition == null)
                {
                    definition = builder.Set(property.Name, property.GetValue(item));
                }
                else
                {
                    definition = definition.Set(property.Name, property.GetValue(item));
                }
            }
            _mongoCollection.UpdateOne(specification, definition);
        }

        public virtual async Task<long> UpdateMany(IEnumerable<T> items)
        {
            var properties = typeof(T).GetProperties();
            var updates = new List<WriteModel<T>>();
            var filterBuilder = Builders<T>.Filter;
            
            foreach(var item in items)
            {
                foreach (var property in properties)
                {
                    var filter = filterBuilder.Eq(property.Name, property.GetValue(item));
                    updates.Add(new ReplaceOneModel<T>(filter, item));
                    break;
                }
            }

            BulkWriteResult result = await _mongoCollection.BulkWriteAsync(updates);
            return result.ModifiedCount;
        }

        public void DeleteAll(Expression<Func<T, bool>> specification)
        {
            _mongoCollection.DeleteMany(specification);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> specification)
        {
            var data = await _mongoCollection.FindAsync(specification);
            return data.FirstOrDefault();
        }

        //protected virtual IMongoCollection<T> GetDbCollection()
        //{


        //    var database = _dbClient.GetDatabase("Flutter-Project");
        //    var collection = database.GetCollection<T>(typeof(T).Name);

        //    return collection;
        //}

    }
}
