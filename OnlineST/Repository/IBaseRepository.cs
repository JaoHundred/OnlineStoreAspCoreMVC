using OnlineST.Models;
using OnlineST.Models.Pagination;
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

        Task<PaginatedCollection<T>> GetAllDataAsync(int pageNumber, int elementsPerPage = 20);

        Task<IEnumerable<T>> GetAllDataAsync();

        T FindData(int id);
    }
}
