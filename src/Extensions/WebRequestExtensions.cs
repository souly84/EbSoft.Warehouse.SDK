using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.Extensions
{
    public static class WebRequestExtensions
    {
        public static async Task<T> ReadAsync<T>(this IWebRequest request)
        {
            string content = string.Empty;
            try
            {
                content = await request
                   .ReadAsStringAsync()
                   .ConfigureAwait(false);
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)content;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                var invalidOperationException = new InvalidOperationException(
                    $"Web Request error occured for {request}",
                    ex
                );
                invalidOperationException.Data["Content"] = content;
                throw invalidOperationException;
            }
        }

        public static IWebRequest WithBody(this IWebRequest request, JObject body)
        {
            return request.WithBody(new SimpleString(body.ToString()));
        }

        public static async Task<IList<T>> SelectAsync<T>(
            this IWebRequest request,
            Func<JObject, T> map)
        {
            var response = await request
                .ReadAsync<List<JObject>>()
                .ConfigureAwait(false);

            return response.Select(map).ToList();
        }

        public static Dictionary<string, string> ToQueryParams(this IFilter filter)
        {
            return new Dictionary<string, string>(
                filter
                    .ToParams()
                    .Select(pair => new KeyValuePair<string, string>(pair.Key, pair.Value.ToString()))
            );
        }

        public static IWebRequest WithFilter(this IWebRequest request, IFilter filter)
        {
            return request.WithQueryParams(filter.ToQueryParams());
        }
    }
}
