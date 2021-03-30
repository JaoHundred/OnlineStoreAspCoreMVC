using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

        public UserType selectedUserType { get; set; }

        public IEnumerable<SelectedUserTypeViewModel> SelectedUserTypes { get; set; }

    }
}
