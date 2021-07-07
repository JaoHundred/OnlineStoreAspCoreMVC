using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.ViewModel.Modal;
using OnlineST.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.UTIL;

namespace OnlineST.Controllers
{
    public class ModalController : Controller
    {
        public ModalController(IBaseRepository<Product> productRepository, CartProductRepository cartProductRepository)
        {
            _productRepository = productRepository;
            _cartProductRepository = cartProductRepository;
        }

        private readonly IBaseRepository<Product> _productRepository;
        private readonly CartProductRepository _cartProductRepository;

        [HttpGet]
        public IActionResult ProductsOpenModal(int id)
        {
            var product = _productRepository.FindData(id);

            if (product is null)
                return StatusCode(204);

            var modalViewModel = product.ToModalViewModel("Products", "ConfirmDelete");
            
            return new JsonResult(modalViewModel);
        }

        [HttpGet]
        public IActionResult CartOpenModal(int id)
        {
            var cartProduct = _cartProductRepository.FindCartProduct(id);

            if (cartProduct is null)
                return StatusCode(204);

            var modalViewModel = cartProduct.Product.ToModalViewModel("ShoppingCart", "ConfirmDelete");

            return new JsonResult(modalViewModel);
        }
    }
}
