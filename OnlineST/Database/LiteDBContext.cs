using LiteDB;
using OnlineST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Database
{
    public class LiteDBContext : ILiteDBContext, IDisposable
    {
        public LiteDatabase LiteDatabase { get; }

        public LiteDBContext()
        {
            var bsonMapper = BsonMapper.Global;

            bsonMapper.Entity<Product>().Id(p => p.Id);

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "database.db");

            LiteDatabase = new LiteDatabase($"Filename={fullPath};connection=shared", bsonMapper);
        }

        public void Dispose()
        {
            LiteDatabase.Dispose();
        }
    }
}
