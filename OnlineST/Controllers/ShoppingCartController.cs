using Microsoft.AspNetCore.Mvc;
using OnlineST.Filters;
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

            if (user is null)
                return Redirect("/Account/Index");

            PaginatedCollection<CartProduct> cartProducts = await _cartProductRepository.GetUserCartProductsAsync(user.Id, page ?? 1, elementsPerPage: 10);

            var paginationModel = new PaginationModel<CartProduct>(cartProducts, "ShoppingCart", nameof(Index));

            return View(paginationModel);
        }

        [Route("/Carrinho/img/{id}")]
        public IActionResult ConvertToImageSRC(int id)
        {
            try
            {
                CartProduct cartProduct = _cartProductRepository.FindCartProduct(id);
                FileContentResult fileContentResult = File(cartProduct.Product.ImageBytes, "image/png");

                return fileContentResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public IActionResult ConfirmDelete(int id)
        {
            try
            {
                CartProduct cartProduct = _cartProductRepository.FindCartProduct(id);

                if (cartProduct.Amount > 1)
                {
                    cartProduct.Amount--;
                    _cartProductRepository.Update(cartProduct, cartProduct.Id);
                }
                else
                {
                    User user = _userSessionService.TryGetUserSessionByEmail();

                    if (user is null)
                        return BadRequest("Usuário não encontrado");

                    _cartProductRepository.Delete(user, id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
