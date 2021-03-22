using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineST.Services
{
    public static class EncryptionService
    {
        public static byte[] GenerateSalt(int length)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return bytes;
        }

        public static byte[] GenerateHash(byte[] password, byte[] salt, int iterations = 10, int length = 10)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(length);
            }
        }

        public static byte[] ToASCIIBytes(this string password)
        {
            return Encoding.ASCII.GetBytes(password);
        }
    }
}
