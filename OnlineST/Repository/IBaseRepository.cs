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

        PaginatedCollection<T> GetAllData(int pageNumber, int elementsPerPage = 20);

        IEnumerable<T> GetAllData();

        T FindData(int id);
    }
}
