using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Amount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 1000)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile FormImage { get; set; }

       
    }
}
