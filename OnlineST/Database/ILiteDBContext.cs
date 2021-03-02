using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Database
{
    public interface ILiteDBContext
    {
        public LiteDatabase LiteDatabase { get; }
    }
}
