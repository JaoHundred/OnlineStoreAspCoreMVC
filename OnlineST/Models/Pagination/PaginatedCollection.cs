using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.Pagination
{
    public class PaginatedCollection<T> : IPaginatedCollection<T> where T : IPersistableObject
    {
        public PaginatedCollection(IEnumerable<T> collection, int total, int pageNumber, int pageSize)
        {
            Collection = collection;
            PageNumber = pageNumber;
            PageSize = pageSize;

            TotalPages = (int)Math.Ceiling((total / (double)pageSize));
        }

        public IEnumerable<T> Collection { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public bool PreviousPage { get => PageNumber > 1; }
        public bool NextPage { get => PageNumber < TotalPages; }
    }
}

