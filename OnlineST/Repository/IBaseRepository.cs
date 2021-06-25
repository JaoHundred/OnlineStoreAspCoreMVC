using OnlineST.Models;
using OnlineST.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineST.Repository
{
    public interface IBaseRepository<T> where T : IPersistableObject
    {
        long Add(T data);
        bool Delete(long id);

        bool Update(T data, long id);

        bool Upsert(T data);

        Task<PaginatedCollection<T>> GetAllDataAsync(int pageNumber, int elementsPerPage = 20);

        Task<IEnumerable<T>> GetAllDataAsync();

        T FindData(long id);
    }
}
