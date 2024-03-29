﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Http;
using OnlineST.Filters;

namespace OnlineST.Controllers
{
    public class AccountController : Controller
    {
        //TODO: fazer funcionalidades de painel/recuperação de senha, troca de senha, troca de email

        public AccountController(IAccountService accountService, UserSessionService sessionService)
        {
            _accountService = accountService;
            _sessionService = sessionService;
        }

        private readonly IAccountService _accountService;
        private readonly UserSessionService _sessionService;

        public IActionResult Index()
        {
            string session = _sessionService.TryGet(UserSessionConst.Email);

            if (string.IsNullOrEmpty(session))
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

            return RedirectToAction(nameof(UserPainel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount([FromForm] UserViewModel userViewModel)
        {
            CreateAccResult accResult = CreateAccResult.None;
            try
            {
                if (ModelState.IsValid)
                    accResult = _accountService.Create(userViewModel);
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
                    MessageToView("Conta criada com sucesso", MessageType.success, FormType.CreateAccount);
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
                //O campo de login não precisa de confirmação de senha
                ModelState.Remove(nameof(userViewModel.ConfirmPassword));
                ModelState.Remove(nameof(userViewModel.RememberMe));

                if (ModelState.IsValid)
                {
                    accResult = _accountService.Login(userViewModel);

                    if (accResult == LogInAccResult.LoggedIn)
                    {
                        //TODO:se o usuário não marcar o RememberMe, ver como fazer, uma espécie de sessão que some depois que fecha o site? usar tempdata?
                        //por hora o rememberme vai ficar desligado

                        //if (userViewModel.RememberMe)
                        _sessionService.Set(UserSessionConst.Email, userViewModel.Email);
                        //else
                        //{

                        //}
                        return Redirect(Url.Action("Index", "Home"));
                    }

                }
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

            return RedirectToAction(nameof(Index));
        }

        //TODO:pesquisar para saber se logout precisa ser passado como post

        public IActionResult Logout()
        {
            User login = _sessionService.TryGetUserSessionByEmail();

            if (login == null)
                return NoContent();

            _sessionService.Delete();

            return RedirectToAction(nameof(Index));
        }

        [AuthorizeUser]
        public IActionResult UserPainel()
        {
            //TODO:montar a view de UserPainel, usar menu lateral a esquerda(o container com a div já está pronto), ao clicar
            //em cada item do menu javascript deve carregar na div "content" o html responsável por troca de senha, troca de email, etc respectivamente

            User user = _sessionService.TryGetUserSessionByEmail();
            return View(user.ToViewModel());
        }
    }
}
