﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Contexts
{
    public interface IReposibity<T>
    {
        void Add(T item);

        void Update(T item, Expression<Func<T, bool>> specification);

        void DeleteAll(Expression<Func<T, bool>> specification);

        Task<long> UpdateMany(IEnumerable<T> items);

        Task<IEnumerable<T>> GetAll();

        Task<T> Get(Expression<Func<T, bool>> specification);

        Task<T> GetAsync(Expression<Func<T, bool>> specification);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> specification);

        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> specification);

        Task<IEnumerable<T>> FindByAsync(FilterDefinition<T> filter);
    }
}
