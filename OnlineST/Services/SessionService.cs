using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.UTIL;

namespace OnlineST.Services
{
    public class SessionService
    {
        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor httpContextAccessor;

        public void Set(string key, string value)
        {
            httpContextAccessor.HttpContext.Session.Set(key, value.ToASCIIBytes());
        }

        public string TryGet(string key)
        {
            byte[] result;
            
            httpContextAccessor.HttpContext.Session.TryGetValue(key, out result);

            if (result != null)
                return result.FromBytes();

            return null;
        }

        public void Delete()
        {
            httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
