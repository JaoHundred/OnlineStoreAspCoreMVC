using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineST.Services;
using OnlineST.UTIL;

namespace OnlineST.Filters
{
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionService = context.HttpContext.RequestServices.GetService(typeof(SessionService)) as SessionService;

            string session = sessionService.TryGet(UserSessionConst.Email);

            if (string.IsNullOrEmpty(session))
                context.Result = new RedirectResult("Index");
        }
    }
}
