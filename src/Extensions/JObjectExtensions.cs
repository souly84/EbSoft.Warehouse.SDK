using Newtonsoft.Json.Linq;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public static class JObjectExtensions
    {
        public static IJsonObject ToJsonBody(this JObject jObject)
        {
            return new JObjectAsJsonBody(jObject);
        }
    }
}
