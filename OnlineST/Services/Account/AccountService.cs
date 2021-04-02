using OnlineST.Models;
using OnlineST.Models.ViewModel;
using OnlineST.Repository;
using OnlineST.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Services.Account
{
    public class AccountService : IAccountService
    {
        public AccountService(IBaseRepository<User> repository)
        {
            this.repository = repository;
        }

        private readonly IBaseRepository<User> repository;

        public CreateAccResult Create(UserViewModel userViewModel)
        {
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

                    return CreateAccResult.AccountCreated;
                }
            }

            return CreateAccResult.IncorrectData;
        }

        public LogInAccResult Login(UserViewModel userViewModel, out string email)
        {
            var user = repository.GetAllData().FirstOrDefault(p => p.Email == userViewModel.Email);
            email = string.Empty;

            if (user != null)
            {
                byte[] password = userViewModel.Password.ToASCIIBytes();
                byte[] salt = user.PasswordSalt;
                byte[] encryptPass = EncryptionService.GenerateHash(password, salt);

                if (user.PasswordHash.SequenceEqual(encryptPass))
                {
                    email = userViewModel.Email;
                    return LogInAccResult.LoggedIn;
                }
            }

            return LogInAccResult.IncorrectLogin;
        }

    }
}
