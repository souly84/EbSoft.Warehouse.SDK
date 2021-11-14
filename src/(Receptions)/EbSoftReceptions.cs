using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Warehouse.Core;
using WebRequest.Elegant;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptions : IReceptions
    {
        private readonly IWebRequest _server;

       
        public EbSoftReceptions(
            IWebRequest server)
        {

            _server= server;
        }

        public async Task<IList<IReception>> ToListAsync()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("filter", "getListCmr");
            dict.Add("date", "2021-10-28");

            var response = await _server
                .WithQueryParams(dict)
                .ReadAsync<List<JObject>>()
                .ConfigureAwait(false);

            return response.Select(
                reception => new EbSoftReception(_server, reception)
            ).ToList<IReception>();
        }
    }
}
