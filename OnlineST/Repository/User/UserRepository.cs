using OnlineST.Database;
using OnlineST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ILiteDBContext liteDBContext) : base(liteDBContext)
        {}

        public User Find(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return _dBContext.LiteDatabase. GetCollection<User>(nameof(User)).Include(p => p.CartProducts).FindOne(p => p.Email == email);
        }
    }
}
