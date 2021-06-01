using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel.Modal
{
    public class ModalViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public int? Id { get; set; }
    }
}
