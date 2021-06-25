using OnlineST.Models;
using OnlineST.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class PaginationExtension 
    {
        public static PaginatedCollection<T> ToPaginationCollection<T>(this IEnumerable<T> collection, int totalElements, int pageNumber, int pageSize) 
            where T: IPersistableObject
        {
            return new PaginatedCollection<T>(collection, totalElements, pageNumber, pageSize);
        }
    }
}
