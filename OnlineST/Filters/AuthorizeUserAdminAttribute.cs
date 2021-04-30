using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineST.Services;
using OnlineST.Models;
using OnlineST.UTIL;

namespace OnlineST.Filters
{
    /// <summary>
    /// checa se o usuário tem cargo de administrador, se tiver deixa ele passar pelo pipeline
    /// </summary>
    public class AuthorizeUserAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionService = context.HttpContext.RequestServices.GetService(typeof(SessionService)) as SessionService;

            string email = sessionService.TryGet(UserSessionConst.Email);

            if (!string.IsNullOrEmpty(email))
            {
                var database = context.HttpContext.RequestServices.GetService(typeof(Repository.UserRepository)) as Repository.UserRepository;

                User user = database.Find(email);

                if (user?.UserType == UserType.Consumer)
                    context.Result = new RedirectResult("Index");
            }
            else
                context.Result = new RedirectResult("Index");
        }
    }
}
