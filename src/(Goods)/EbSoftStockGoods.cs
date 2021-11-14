using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftStockGoods : IGoods
    {
        private readonly IWebRequest _server;
        private readonly string _receptionId;

        public EbSoftStockGoods(
            IWebRequest server,
            string receptionId)
        {
            _server = server;
            _receptionId = receptionId;
        }

        public async Task<IList<IGood>> ToListAsync()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("filter", "getCmrlines");
            dict.Add("id", _receptionId);

            var response = await _server
                .WithQueryParams(dict)
                .ReadAsync<List<JObject>>()
                .ConfigureAwait(false);

            return response.Select(
                good => new EbSoftGood(_server, good)
            ).ToList<IGood>();

        }

        public IGoods With(IFilter filter)
        {
            return this;
        }
    }
}
