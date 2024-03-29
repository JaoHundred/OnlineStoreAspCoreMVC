﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.Pagination
{
    public interface IPaginatedCollection<T>: IPaginatedCollection where T : IPersistableObject
    {
        public IEnumerable<T> Collection { get; }
    }

    public interface IPaginatedCollection
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public bool PreviousPage { get; }
        public bool NextPage { get; }
    }
}
