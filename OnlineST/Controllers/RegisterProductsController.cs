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
            try
            {
                var product = new Product();
                _repository.Upsert(product, product.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
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
            return View();
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

        public IActionResult SetImage(int id)
        {
            //TODO: abrir o seletor de imagem do windows e pegar a imagem setando como bytes

            return View();
        }
    }
}
