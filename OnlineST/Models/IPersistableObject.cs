 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Models
{
    public interface IPersistableObject
    {
        public long Id { get; set; }
    }
}
