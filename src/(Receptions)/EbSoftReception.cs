using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReception : IReception
    {
        private readonly IWebRequest _server;
        private readonly JToken _ebSoftReception;

        public EbSoftReception(
            IWebRequest server,
            JToken ebSoftReception)
        {
            _server = server;
            _ebSoftReception = ebSoftReception;
        }

        public IGoods Goods => new EbSoftStockGoods(_server, _ebSoftReception.Value<string>("id"));

        public Task ConfirmAsync(IGood good)
        {
            return Task.CompletedTask;
        }

        public Task ValidateAsync()
        {
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return _ebSoftReception.ToString();
        }
    }
}
