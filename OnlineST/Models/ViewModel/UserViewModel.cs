using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel
{
    public class UserViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public int SelectedIndex { get; set; }

        [Required]
        public UserType selectedUserType { get; set; }

        public IEnumerable<SelectedUserTypeViewModel> SelectedUserTypes { get; set; }

    }
}
