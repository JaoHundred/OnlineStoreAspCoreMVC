using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.Database;
using OnlineST.Models.ViewModel;
using System.IO;
using OnlineST.UTIL;
using OnlineST.Filters;
using OnlineST.Services;
using OnlineST.Models.Pagination;
using OnlineST.Models.ViewModel.Modal;
using System.Text.Json;

namespace OnlineST.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsController(
            IBaseRepository<Product> productRepository,
            CartProductRepository cartProductRepository,
            UserRepository userRepository,
            UserSessionService userSessionService)
        {
            _productRepository = productRepository;
            _cartProductRepository = cartProductRepository;
            _userRepository = userRepository;
            _userSessionService = userSessionService;
        }

        private readonly IBaseRepository<Product> _productRepository;
        private readonly CartProductRepository _cartProductRepository;
        private readonly UserRepository _userRepository;
        private readonly UserSessionService _userSessionService;

        [HttpGet]
        [Route("Produtos/Page/{page?}")]
        public async Task<IActionResult> Index(int? page)
        {
            PaginatedCollection<Product> products = await _productRepository.GetAllDataAsync(page ?? 1, elementsPerPage: 5);

            var model = new PaginationModel<Product>(products, "Products", nameof(Index));

            return View(model);
        }

        // GET: RegisterProductsController/Details/5
        [Route(nameof(Details))]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        [AuthorizeUserAdmin]
        public IActionResult Product()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeUserAdmin]
        [AutoValidateAntiforgeryToken]
        public IActionResult Product(int id)
        {
            Product product = _productRepository.FindData(id);

            if (product != null)
            {
                var productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Amount = product.Amount,
                    Description = product.Description,
                };

                using (var stream = new MemoryStream(product.ImageBytes))
                {
                    productViewModel.FormImage = new FormFile(stream, 0, stream.Length, "name", product.Name);
                }

                return View(productViewModel);
            }

            return NoContent();

        }

        [HttpPost]
        [AuthorizeUserAdmin]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            return RedirectToAction(nameof(Product), id);
        }

        [HttpPost]
        [AuthorizeUserAdmin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromForm] ProductViewModel productViewModel)
        {
            try
            {
                //TODO: ver como passar o Iformimage para o asp-for do product.cshtml

                ModelState.Remove(nameof(productViewModel.Id));

                if (!ModelState.IsValid)
                {
                    TempData[nameof(ModelState.ErrorCount)] = ModelState.ErrorCount;
                    return RedirectToAction(nameof(Product));
                }

                string extensionType = GetFileExtension(productViewModel.FormImage);

                if (!(extensionType is "png" or "jpg" or "jpeg"))
                    return RedirectToAction(nameof(Product));

                var product = await productViewModel.ToProductAsync();

                _productRepository.Upsert(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Product));
            }
        }

        private string GetFileExtension(IFormFile formFile)
        {
            return new string(formFile.ContentType.Reverse().TakeWhile(p => p is not '/').Reverse().ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserAdmin]
        public IActionResult ConfirmDelete(int id)
        {
            try
            {
                _productRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("/Produto/img/{id}")]
        public IActionResult ConvertToImageSRC(int id)
        {
            try
            {
                Product product = _productRepository.FindData(id);
                FileContentResult fileContentResult = File(product.ImageBytes, "image/png");

                return fileContentResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [AuthorizeUser]
        public IActionResult AddToShoppingCart(int id)
        {
            var user = _userSessionService.TryGetUserSessionByEmail();
            Product product = _productRepository.FindData(id);

            if (product is null)
                return StatusCode(204);

            if (user is null)
                return Redirect("/Account/Index");

            CartProduct existingCartProduct = _cartProductRepository.FindCartProductByUserId(user.Id, product.Id);

            if (existingCartProduct is null)
            {
                var cartProduct = new CartProduct
                {
                    AddedDate = DateTimeOffset.UtcNow,
                    Product = product,
                    Amount = 1,
                };

                long cartId = _cartProductRepository.Upsert(cartProduct);

                cartProduct = _cartProductRepository.FindCartProduct(cartId);

                user.CartProducts.Add(cartProduct);
                
            }
            else
            {
                existingCartProduct.Amount++;
                _cartProductRepository.Update(existingCartProduct, existingCartProduct.Id);
            }


            _userRepository.Update(user, user.Id);

            var messageVM = new MessageViewModel
            {
                Message = $"Produto {product.Name} adicionado ao carrinho",
                MessageType = MessageType.success,
            };

            TempData.PutExt(nameof(MessageViewModel), messageVM);

            return RedirectToAction(nameof(Index));
        }
    }
}
