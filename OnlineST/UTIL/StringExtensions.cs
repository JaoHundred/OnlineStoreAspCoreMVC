using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class StringExtensions
    {
        public static byte[] ToASCIIBytes(this string password)
        {
            return Encoding.ASCII.GetBytes(password);
        }

        public static string FromBytes(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
