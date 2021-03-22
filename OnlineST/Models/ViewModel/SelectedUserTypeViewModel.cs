using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel
{
    public class SelectedUserTypeViewModel
    {

        public SelectedUserTypeViewModel(string userTypeName, UserType userType, bool isSelected = false)
        {
            UserTypeName = userTypeName;
            UserType = userType;
            IsSelected = isSelected;
        }

        public string UserTypeName { get; set; }
        public UserType UserType { get; set; }

        public bool IsSelected { get; set; }
    }
}
