using Microsoft.AspNetCore.Http;
using OnlineST.Models;
using OnlineST.Repository;
using OnlineST.UTIL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Services
{
    public class UserSessionService : SessionService
    {

        public UserSessionService(IHttpContextAccessor httpContextAccessor, UserRepository repository) : base(httpContextAccessor)
        {
            _repository = repository;
        }

        private readonly UserRepository _repository;

        public User TryGetUserSessionByEmail()
        {
            string email = TryGet(UserSessionConst.Email);

            if (string.IsNullOrWhiteSpace(email))
                return null;

            return _repository.Find(email);
        }
    }
}
