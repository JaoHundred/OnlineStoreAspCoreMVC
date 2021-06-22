using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.Pagination;
using OnlineST.Repository;
using OnlineST.Services;
using OnlineST.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Controllers
{
    public class ShoppingCartController : Controller
    {

        public ShoppingCartController(CartProductRepository cartProductRepository, UserSessionService userSessionService)
        {
            _cartProductRepository = cartProductRepository;
            _userSessionService = userSessionService;
        }

        private readonly CartProductRepository _cartProductRepository;
        private readonly UserSessionService _userSessionService;

        public async Task<IActionResult> Index(int? page)
        {
            var user = _userSessionService.TryGetUserSessionByEmail();

            if(user is null)
                return Redirect("/Account/Index");

            PaginatedCollection<CartProduct> paginatedCollection = await _cartProductRepository.GetUserCartProductsAsync(user.Id, page ?? 1, 10);

            return View(paginatedCollection);
        }
    }
}
