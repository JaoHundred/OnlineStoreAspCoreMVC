using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.Pagination
{
    public interface IPaginationModel
    {
        public string ControllerName { get; }
        public string ActionName { get; }
        public IPaginatedCollection PaginatedCollection { get; }
    }
}
