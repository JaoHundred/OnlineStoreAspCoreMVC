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
        public AccountService(UserRepository repository)
        {
            this.repository = repository;
        }

        private readonly UserRepository repository;

        public CreateAccResult Create(UserViewModel userViewModel)
        {
            byte[] password = userViewModel.Password.ToASCIIBytes();
            byte[] confirmPassword = userViewModel.ConfirmPassword.ToASCIIBytes();

            if (password.SequenceEqual(confirmPassword))
            {
                User user = repository.Find(userViewModel.Email);

                if (user is null)
                {
                    byte[] salt = EncryptionService.GenerateSalt(10);
                    byte[] encryptPass = EncryptionService.GenerateHash(password, salt);

                    user = new User
                    {
                        Email = userViewModel.Email,
                        UserType = userViewModel.selectedUserType,
                        PasswordHash = encryptPass,
                        PasswordSalt = salt,
                        PasswordIterations = 10,
                    };

                    repository.Add(user);

                    return CreateAccResult.AccountCreated;
                }
            }

            return CreateAccResult.IncorrectData;
        }

        public LogInAccResult Login(UserViewModel userViewModel)
        {
            User user = repository.Find(userViewModel.Email);

            if (user is not null)
            {
                byte[] password = userViewModel.Password.ToASCIIBytes();
                byte[] salt = user.PasswordSalt;
                byte[] encryptPass = EncryptionService.GenerateHash(password, salt);

                if (user.PasswordHash.SequenceEqual(encryptPass))
                {
                    return LogInAccResult.LoggedIn;
                }
            }

            return LogInAccResult.IncorrectLogin;
        }

    }
}
