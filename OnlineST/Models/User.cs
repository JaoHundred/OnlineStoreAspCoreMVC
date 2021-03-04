using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public class User
    {
        public User()
        {

        }

        public int Id { get; set; }
        public UserType UserType { get; set; } = UserType.None;
        public string Email { get; set; }
    }

    public enum UserType
    {
        None = 0,
        Admin = 1,
        Consumer = 2,
    };
}
