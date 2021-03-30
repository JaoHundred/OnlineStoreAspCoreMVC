using OnlineST.Models.ViewModel;
using OnlineST.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Services.Account
{
    public interface IAccountService
    {
        CreateAccResult Create(UserViewModel userViewModel);
        LogInAccResult Login(UserViewModel userViewModel);
    }
}
