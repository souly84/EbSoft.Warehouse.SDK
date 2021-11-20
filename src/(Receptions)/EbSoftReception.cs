using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods;
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

        public override string ToString()
        {
            return _ebSoftReception.ToString();
        }

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            throw new System.NotImplementedException();
        }
    }
}
