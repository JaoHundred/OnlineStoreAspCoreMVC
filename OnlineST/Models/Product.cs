﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public class Product : IPersistableObject, INamedObject
    {
        public Product()
        {

        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
