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

namespace OnlineST.Controllers
{
    public class ProductsController : Controller
    {
        public ProductsController(IBaseRepository<Product> productRepository, UserSessionService userSessionService)
        {
            _productrepository = productRepository;
            _userSessionService = userSessionService;
        }

        private readonly IBaseRepository<Product> _productrepository;
        private readonly UserSessionService _userSessionService;

        // GET: RegisterProductsController
        public ActionResult Index()
        {
            User user = _userSessionService.TryGetUserSession(UserSessionConst.Email);

            if (user != null)
                TempData.PutExt(UserSessionConst.Email, user);

            var products = _productrepository.GetAllData();

            return View(products);
        }

        // GET: RegisterProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterProductsController/Create
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

                _productrepository.Add(product);

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

        // POST: RegisterProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegisterProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RegisterProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RegisterProductsController/Delete/5
        [AuthorizeUserAdmin]
        public ActionResult Delete(int id)
        {
            try
            {
                _productrepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: RegisterProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult ConvertToImageSRC(int id)
        {
            try
            {
                Product product = _productrepository.GetAllData().First(p => p.Id == id);
                FileContentResult fileContentResult = File(product.ImageBytes, "image/png");

                return fileContentResult;
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
