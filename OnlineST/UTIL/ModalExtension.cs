using OnlineST.Models;
using OnlineST.Models.ViewModel.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class ModalExtension
    {
        public static ModalViewModel ToModalViewModel(this INamedObject persistableObject, string controller, string action)
        {
            return new ModalViewModel
            {
                Controller = controller,
                Action = action,
                Title = "Um item será deletado, deseja continuar?",
                Message = $"Deseja deletar {persistableObject.Name}?",
                Id = persistableObject.Id,
            };
        }
    }
}
