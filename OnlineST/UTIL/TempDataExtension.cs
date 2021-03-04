using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineST.UTIL
{
    public static class TempDataExtension
    {
        public static void PutExt<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T GetExt<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            tempData.TryGetValue(key, out object data);
            return data == null ? null : JsonSerializer.Deserialize<T>((string)data);
        }

        public static T PeekExt<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object data = tempData.Peek(key);
            return data == null ? null : JsonSerializer.Deserialize<T>((string)data);
        }
    }
}
