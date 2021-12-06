using System.Collections.Generic;
using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods.Storages;
using WebRequest.Elegant;
using EbSoft.Warehouse.SDK.Extensions;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGoodMovement : IMovement
    {
        private readonly IWebRequest _server;
        private readonly IWarehouseGood _good;
        private readonly IStorage _fromStorage;

        public EbSoftGoodMovement(IWebRequest server, IWarehouseGood good)
            : this(server, good, new IncorrectStorage())
        {
        }

        public EbSoftGoodMovement(
            IWebRequest server,
            IWarehouseGood good,
            IStorage fromStorage)
        {
            _server = server;
            _good = good;
            _fromStorage = fromStorage;
        }

        public IMovement From(IStorage storage)
        {
            return new EbSoftGoodMovement(
                _server,
                _good,
                storage
            );
        }

        public Task MoveToAsync(IStorage storage, int quantity)
        {
            var ean = _good.ToDictionary().Value<string>("Ean");
            return _server.WithQueryParams(new Dictionary<string, string>
            {
                { "filter", "assignProductTo" },
                { "ean", ean },
            }).WithBody(
                new JObject(
                    new JProperty("ean", ean),
                    new JProperty("origin", _fromStorage.ToDictionary().Value<string>("Number")),
                    new JProperty("destination", storage.ToDictionary().Value<string>("Number")),
                    new JProperty("quantity", quantity)
                )
            ).EnsureSuccessAsync();
        }
    }
}
