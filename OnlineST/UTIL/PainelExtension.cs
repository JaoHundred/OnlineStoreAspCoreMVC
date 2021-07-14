using OnlineST.Models;
using OnlineST.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class PainelExtension
    {
        public static PainelViewModel ToViewModel(this User user)
        {
            return new PainelViewModel
            {
                //TODO:preencher as propriedades
                Id = user.Id,
                Email = user.Email,
            };
        }

        public static User ToModel(this PainelViewModel painelViewModel)
        {
            return new User
            {
                Id = painelViewModel.Id,
                Email = painelViewModel.Email,
                //TODO:preencher as propriedades
            };
        }
    }
}
