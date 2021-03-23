using Microsoft.AspNetCore.Mvc;
using OnlineST.Models;
using OnlineST.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.Services;
using OnlineST.Repository;

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
            //TODO: ver como o radiobutton funciona
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction(nameof(Index));

                byte[] password = userViewModel.Password.ToASCIIBytes();
                byte[] confirmPassword = userViewModel.Password.ToASCIIBytes();

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
                            UserType = (UserType)userViewModel.selectedUserType,
                            PasswordHash = encryptPass,
                            PasswordSalt = salt,
                            PasswordIterations = 10,
                        };

                        repository.Add(newUser);

                        //TODO:avisar que foi cadastrado com sucesso

                    }


                    //TODO: avisar que já existe este email acadastrado
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

            }

            return new UnprocessableEntityResult();
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
