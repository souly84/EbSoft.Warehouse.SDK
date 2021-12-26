using System.Collections.Generic;
using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods.Storages;
using WebRequest.Elegant;
using EbSoft.Warehouse.SDK.Extensions;
using System.Net.Http;

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
            return _server
                .WithMethod(HttpMethod.Post)
                .WithBody(
                    new Dictionary<string, IJsonObject>
                    {
                        { "filter", new SimpleString("moveProductWarehouse") },
                        { "json", new JObject(
                            new JProperty("ean", _good.ToDictionary().Value<JObject>("Data").Value<string>("ean")),
                            new JProperty("origin", _fromStorage.ToDictionary().Value<string>("Number")),
                            new JProperty("destination", storage.ToDictionary().Value<string>("Number")),
                            new JProperty("quantity", quantity)
                          ).ToJsonBody()
                        }
                    }
                )
                .EnsureSuccessAsync();
        }
    }
}
