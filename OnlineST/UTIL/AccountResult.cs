using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public enum CreateAccResult
    {
        None,
        AccountCreated,
        EmptyFields,
        IncorrectData,
    };

    public enum LogInAccResult
    {
        None,
        LoggedIn,
        EmptyFields,
        IncorrectLogin,
    };
}
