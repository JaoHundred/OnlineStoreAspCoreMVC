using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public interface INamedObject : IPersistableObject
    {
        public string Name { get; set; }
    }
}
