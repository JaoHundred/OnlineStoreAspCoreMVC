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

        [HttpGet]
        [Route("Carrinho/Page/{page?}")]
        public async Task<IActionResult> Index(int? page)
        {
            var user = _userSessionService.TryGetUserSessionByEmail();

            if(user is null)
                return Redirect("/Account/Index");

            PaginatedCollection<CartProduct> cartProducts = await _cartProductRepository.GetUserCartProductsAsync(user.Id, page ?? 1, elementsPerPage: 2);

            var paginationModel = new PaginationModel<CartProduct>(cartProducts, "ShoppingCart", nameof(Index));

            //TODO: carregar a imagem dos respectivos produtos na view

            return View(paginationModel);
        }
    }
}
