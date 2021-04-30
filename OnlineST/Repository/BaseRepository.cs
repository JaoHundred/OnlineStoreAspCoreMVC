using OnlineST.Database;
using OnlineST.Models;
using OnlineST.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.UTIL;

namespace OnlineST.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        public BaseRepository(ILiteDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        protected ILiteDBContext _dBContext;

        public int Add(T data)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().Insert(data);
        }

        public bool Delete(int id)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().Delete(id);
        }

        public bool Update(T data, int id)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().Update(id, data);
        }

        public async Task<PaginatedCollection<T>> GetAllDataAsync(int pageNumber, int elementsPerPage = 20)
        {
            int skip = (pageNumber - 1) * elementsPerPage;

            var task = Task.Run(() =>
            {
                return _dBContext.LiteDatabase
                .GetCollection<T>()
                .Query()
                .Offset(skip)
                .Limit(elementsPerPage);
            });

            var result = await task;

            PaginatedCollection<T> collection = result.ToEnumerable().ToPaginationCollection(pageNumber);
            //TODO: testar esse método quando estiver com mais dados de produtos no banco
            return collection;
        }

        public async Task<IEnumerable<T>> GetAllDataAsync()
        {
            var task = Task.Run(() =>
            {
                return _dBContext.LiteDatabase.GetCollection<T>().FindAll();
            });

            return await task;
        }

        public T FindData(int id)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().FindById(id);
        }

        public bool Upsert(T data, int id)
        {
            var obj = FindData(id);

            if (obj == null)
            {
                Add(data);
                return true;
            }
            else
            {
                Update(data, id);
                return true;
            }
        }


    }
}
