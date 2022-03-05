using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Contexts
{
    public interface  IResposibity<T>
    {
        void Add(T item);

        void Update(T item);

        IEnumerable<T> GetAll();

        Task<T> Get(string Id);

        Task<T> GetAsync(Expression<Func<T, bool>> specification);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> specification);

        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification);
    }
}
