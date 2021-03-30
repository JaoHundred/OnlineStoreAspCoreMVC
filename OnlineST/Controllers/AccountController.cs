using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.Services;
using OnlineST.Repository;
using OnlineST.UTIL;
using OnlineST.Services.Account;

namespace OnlineST.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        private readonly IAccountService accountService;

        public IActionResult Index()
        {
            var userViewModel = new UserViewModel
            {
                SelectedUserTypes = new List<SelectedUserTypeViewModel>
                {
                  new SelectedUserTypeViewModel("Consumidor", UserType.Consumer),
                  new SelectedUserTypeViewModel("Administrador", UserType.Admin, isSelected:true),
                },
            };
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount([FromForm] UserViewModel userViewModel)
        {
            CreateAccResult accResult = CreateAccResult.None;
            try
            {
                if (ModelState.IsValid)
                    accResult = accountService.Create(userViewModel);
                else
                    accResult = CreateAccResult.EmptyFields;
            }
            catch (Exception ex)
            {
                //TODO:armazenar logo de erros
            }

            switch (accResult)
            {
                case CreateAccResult.AccountCreated:
                    MessageToView("Conta criada com sucesso", MessageType.success , FormType.CreateAccount);
                    break;
                case CreateAccResult.EmptyFields:
                    MessageToView("Existem campos não preenchidos", MessageType.danger, FormType.CreateAccount);
                    break;
                case CreateAccResult.IncorrectData:
                    MessageToView("Dados incorretos", MessageType.danger, FormType.CreateAccount);
                    break;
                case CreateAccResult.None:
                    MessageToView("Erro desconhecido", MessageType.danger, FormType.CreateAccount);
                    break;
            }

            return RedirectToAction(nameof(Index));
        }

        private void MessageToView(string message, MessageType messageType, FormType formType)
        {
            var messageTypeViewModel = new MessageViewModel
            {
                FormType = formType,
                Message = message,
                MessageType = messageType,
            };

            TempData.PutExt(nameof(MessageViewModel), messageTypeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] UserViewModel userViewModel)
        {
            LogInAccResult accResult = LogInAccResult.None;
            try
            {
                if (ModelState.IsValid && string.IsNullOrEmpty(userViewModel.ConfirmPassword))
                    accResult = accountService.Login(userViewModel);
                else
                    accResult = LogInAccResult.EmptyFields;
            }
            catch (Exception ex)
            {
                //TODO: armazenar logs
            }

            switch (accResult)
            {
                case LogInAccResult.None:
                    MessageToView("Erro desconhecido", MessageType.danger, FormType.Login);
                    break;
                case LogInAccResult.EmptyFields:
                    MessageToView("Existem campos não preenchidos", MessageType.danger, FormType.Login);
                    break;
                case LogInAccResult.IncorrectLogin:
                    MessageToView("Login incorreto", MessageType.danger, FormType.Login);
                    break;
                default:
                    break;
            }

            if (accResult == LogInAccResult.LoggedIn)
                return Redirect("/Home/Index");

            return RedirectToAction(nameof(Index));
        }
    }
}
