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

namespace OnlineST.Controllers
{
    public class AccountController : Controller
    {

        public AccountController(IBaseRepository<User> repository)
        {
            this.repository = repository;
        }

        private readonly IBaseRepository<User> repository;

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
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.TryAdd("RegisterError", true);
                    return RedirectToAction(nameof(Index));
                }

                byte[] password = userViewModel.Password.ToASCIIBytes();
                byte[] confirmPassword = userViewModel.ConfirmPassword.ToASCIIBytes();

                if (password.SequenceEqual(confirmPassword))
                {
                    bool isNewUser = !repository.GetAllData().ToList().Exists(p => p.Email == userViewModel.Email);

                    if (isNewUser)
                    {
                        byte[] salt = EncryptionService.GenerateSalt(10);
                        byte[] encryptPass = EncryptionService.GenerateHash(password, salt);

                        var newUser = new User
                        {
                            Email = userViewModel.Email,
                            UserType = userViewModel.selectedUserType,
                            PasswordHash = encryptPass,
                            PasswordSalt = salt,
                            PasswordIterations = 10,
                        };

                        repository.Add(newUser);

                        TempData.TryAdd("UserRegistered", true);
                        return RedirectToAction(nameof(Index));
                    }

                    TempData.TryAdd("AccountAlreadyExists", true);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData.TryAdd("ConfirmationDontMatch", true);
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                //TODO:armazenar logo de erros
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] UserViewModel userViewModel)
        {
            try
            {
                if (!ModelState.IsValid && !string.IsNullOrEmpty(userViewModel.ConfirmPassword))
                {
                    TempData.TryAdd("LoginErrorEmptyFields", true);
                    return RedirectToAction(nameof(Index));
                }

                var user = repository.GetAllData().FirstOrDefault(p => p.Email == userViewModel.Email);

                if (user != null)
                {
                    byte[] password = userViewModel.Password.ToASCIIBytes();
                    byte[] salt = user.PasswordSalt;
                    byte[] encryptPass = EncryptionService.GenerateHash(password, salt);

                    if (user.PasswordHash.SequenceEqual(encryptPass))
                    {
                        //TODO: guardar a seção do usuário e ir para a home
                        return RedirectPermanent("/Home/Index");
                    }

                    TempData["IncorrectLogin"] = true;
                    return RedirectToAction(nameof(Index));
                }

                TempData["UserDontExist"] = true;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //TODO: armazenar logs
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
