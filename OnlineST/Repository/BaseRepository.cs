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
    public class BaseRepository<T> : IBaseRepository<T> where T : IPersistableObject
    {
        public BaseRepository(ILiteDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        protected ILiteDBContext _dBContext;

        public long Add(T data)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().Insert(data);
        }

        public bool Delete(long id)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().Delete(id);
        }

        public bool Update(T data, long id)
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

            int elementsCount = _dBContext.LiteDatabase.GetCollection<T>().Count();

            var result = await task;

            PaginatedCollection<T> collection = result.ToEnumerable().ToPaginationCollection(elementsCount, pageNumber, elementsPerPage);
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

        public T FindData(long id)
        {
            return _dBContext.LiteDatabase.GetCollection<T>().FindById(id);
        }

        public bool Upsert(T data)
        {
            int count = _dBContext.LiteDatabase.GetCollection<T>().Count(p => p.Id == data.Id);

            if(count > 0)
            {
                Update(data, data.Id);
                return true;
            }
            else if(count == 0)
            {
                Add(data);
                return true;
            }

            return false;
           
        }
    }
}
