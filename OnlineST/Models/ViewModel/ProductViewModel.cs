using Microsoft.AspNetCore.Http;
using OnlineST.UTIL;
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


        public async Task<Product> ToProductAsync()
        {
            return new Product
            {
                Id = this.Id,
                Name = this.Name,
                Price = this.Price,
                Amount = this.Amount,
                Description = this.Description,
                ImageBytes = await this.FormImage.ConvertToBytesAsync(),
            };
        }
    }
}
