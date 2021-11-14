using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Extensions
{
    public static class WebRequestExtensions
    {
        public static async Task<T> ReadAsync<T>(this IWebRequest request)
        {
            var response = await request
               .GetResponseAsync()
               .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content
               .ReadAsStringAsync()
               .ConfigureAwait(false);
            if (typeof(T) == typeof(string))
            {
                return (T)(object)content;
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
