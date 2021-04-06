using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineST.Models;
using OnlineST.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.UTIL;

namespace OnlineST.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(ILogger<HomeController> logger, SessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        private readonly ILogger<HomeController> _logger;
        private readonly SessionService _sessionService;

        public IActionResult Index()
        {
            string email = _sessionService.TryGet(UserSessionConst.Email);

            //TODO:feito dessa forma para não passar um nome de view para o método view, só para ver se está funcionando a sessão
            return View(email as object );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
