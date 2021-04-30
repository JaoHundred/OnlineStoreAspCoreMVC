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

namespace OnlineST.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsController(IBaseRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        private readonly IBaseRepository<Product> _productRepository;

        [HttpGet]
        [Route("/Page/{page?}")]
        public async Task<IActionResult> Index(int? page)
        {
            //TODO:ao clicar nos componentes de paginação(a fazer) chamar o index e passar o número correto da página

            PaginatedCollection<Product> products = await _productRepository.GetAllDataAsync(page ?? 1, elementsPerPage: 10);

            return View(products);
        }

        // GET: RegisterProductsController/Details/5
        [Route(nameof(Details))]
        public ActionResult Details(int id)
        {
            return View();
        }

        [AuthorizeUserAdmin]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeUserAdmin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] ProductViewModel productViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[nameof(ModelState.ErrorCount)] = ModelState.ErrorCount;
                    return RedirectToAction(nameof(Create));
                }

                string extensionType = GetFileExtension(productViewModel.FormImage);

                if (!(extensionType is "png" or "jpg" or "jpeg"))
                    return RedirectToAction(nameof(Create));

                var product = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Description = productViewModel.Description,
                    ImageBytes = await productViewModel.FormImage.ConvertToBytesAsync(),
                };

                _productRepository.Add(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Create));
            }
        }

        private string GetFileExtension(IFormFile formFile)
        {
            return new string(formFile.ContentType.Reverse().TakeWhile(p => p is not '/').Reverse().ToArray());
        }


        // GET: RegisterProductsController/Edit/5
        [AuthorizeUserAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            return View();
        }


        //TODO:ValidateAntiForgeryToken só funciona com post, procurar se é possível fazer um post sem forms method post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUserAdmin]
        public IActionResult Delete(int id)
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

        [Route("/img/{name}")]
        [Route(nameof(ConvertToImageSRC))]
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
    }
}
