using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public class User : IPersistableObject
    {
        public User()
        {
            if (CartProducts is null)
                CartProducts = new List<CartProduct>();
        }

        public long Id { get; set; }
        public UserType UserType { get; set; } = UserType.None;
        public string Email { get; set; }
        public byte[] PasswordHash { get;  set; }
        public byte[] PasswordSalt { get;  set; }
        public int PasswordIterations { get;  set; }
        public List<CartProduct> CartProducts { get; set; }
    }

    public enum UserType
    {
        None = -1,
        Consumer = 0,
        Admin = 1,
    };
}
