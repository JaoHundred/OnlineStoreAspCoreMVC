using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Repository
{
    public interface IBaseRepository<T>
    {
        int Add(T data);
        bool Delete(int id);

        bool Update(T data, int id);

        bool Upsert(T data, int id);

        IEnumerable<T> GetAllData();

        T FindData(int id);
    }
}
