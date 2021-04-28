using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.Pagination
{
    public class PaginatedCollection<T> 
    {
        public PaginatedCollection(IEnumerable<T> collection, int pageNumber)
        {
            Collection = collection;
            PageNumber = pageNumber;
        }

        public IEnumerable<T> Collection { get; }
        public int PageNumber { get; }
    }
}

