using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.Pagination
{
    public class PaginationModel<T> : IPaginationModel where T: IPersistableObject
    {
        public PaginationModel(IPaginatedCollection<T> paginatedCollection, string controllerName, string actionName)
        {
            PaginatedCollection = paginatedCollection;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public IPaginatedCollection PaginatedCollection { get; }
        public string ControllerName { get; }
        public string ActionName { get; }
    }
}
