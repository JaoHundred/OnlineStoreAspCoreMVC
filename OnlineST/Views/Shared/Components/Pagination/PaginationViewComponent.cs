using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.Models.Pagination;
using OnlineST.Models;

namespace OnlineST.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        //TODO:ver como será montado a paginação
        public async Task<IViewComponentResult> InvokeAsync(PaginatedCollection<Product> paginatedCollection)
        {
            return View(paginatedCollection);
        }
    }
}
