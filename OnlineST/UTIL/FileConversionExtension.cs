using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class FileConversionExtension
    {
        public static async Task<byte[]> ConvertToBytesAsync(this IFormFile formFile)
        {
            byte[] byteImage;
            using (var memStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memStream);
                byteImage = memStream.ToArray();
            }

            return byteImage;
        }
    }
}
