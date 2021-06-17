using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public class CartProduct : IPersistableObject
    {
        public CartProduct()
        {

        }

        public int Id { get; set; }
        public DateTimeOffset AddedDate { get; set; }

        public Product Product { get; set; }
    }
}
