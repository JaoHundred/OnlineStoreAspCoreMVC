using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.ViewModel.Modal;
using OnlineST.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Controllers
{
    public class ModalController : Controller
    {
        public ModalController(IBaseRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        private readonly IBaseRepository<Product> _productRepository;

        [HttpGet]
        public IActionResult OpenModal(int id)
        {
            var product = _productRepository.FindData(id);

            if (product is null)
                return StatusCode(204);

            var modalViewModel = new ModalViewModel
            {
                Controller = nameof(ProductsController).Replace("Controller", string.Empty),
                Action = "ConfirmDelete",
                Title = "Um item será deletado, deseja continuar?",
                Message = $"Deseja deletar {product.Name}?",
                Id = id,
            };

            return new JsonResult(modalViewModel);
        }
    }
}
