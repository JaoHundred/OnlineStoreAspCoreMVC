using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount([FromForm] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid) 
                return RedirectToAction(nameof(Index));
            
            //TODO:fazer o sistema de criar conta, criar as classes de criptografia

            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            //TODO:fazer o sistema de login, usar sessões

            return new EmptyResult();
        }
    }
}
