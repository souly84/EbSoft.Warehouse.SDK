using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGood : IGood
    {
        private readonly IWebRequest _server;
        private readonly JToken _ebSoftGood;

        public EbSoftGood(
            IWebRequest server,
            JToken ebSoftGood)
        {
            _server = server;
            _ebSoftGood = ebSoftGood;
        }
    }
}
