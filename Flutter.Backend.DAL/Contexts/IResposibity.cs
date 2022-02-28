using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.DAL.Contexts
{
    public interface  IResposibity<T> where T : class
    {
        void Add(T item);

        void Update(T item);
    }
}
