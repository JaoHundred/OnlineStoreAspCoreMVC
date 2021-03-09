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

namespace OnlineST.Controllers
{
    public class RegisterProductsController : Controller
    {

        public RegisterProductsController(IBaseRepository<Product> repository)
        {
            _repository = repository;
        }

        private readonly IBaseRepository<Product> _repository;

        // GET: RegisterProductsController
        public ActionResult Index()
        {
            var products = _repository.GetAllData();

            foreach (var item in products)
            {

            }

            return View(products);
        }

        // GET: RegisterProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RegisterProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] ProductViewModel productViewModel)
        {
            try
            {
                //TODO:colocar o código de extração de extensão em uma classe de utilitário
                string extensionType = new string(productViewModel.FormImage.ContentType.Reverse().TakeWhile(p => p is not '/').Reverse().ToArray());

                if(!(extensionType is "png" or "jpg" or "jpeg"))
                    return RedirectToAction(nameof(Create));

                //TODO:colocar o código de conversão de IformFile para bytes em uma classe de utilitário
                byte[] byteImage;
                using (var memStream = new MemoryStream())
                {
                    await productViewModel.FormImage.CopyToAsync(memStream);
                    byteImage = memStream.ToArray();
                }

                //TODO:colocar o código conversão da viewModel para o produto em classe utilitária
                var product = new Product
                {
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Description = productViewModel.Description,
                    ImageBytes = byteImage,
                };

                _repository.Add(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Create));
            }
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
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);

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
                Product product = _repository.GetAllData().First(p => p.Id == id);
                FileContentResult fileContentResult= File(product.ImageBytes, "image/png");
                
                return fileContentResult;
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}
