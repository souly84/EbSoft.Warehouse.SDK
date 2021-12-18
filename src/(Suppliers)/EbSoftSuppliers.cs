using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftSuppliers : IEntities<ISupplier>
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftSuppliers(IWebRequest server)
            : this(server, new SuppliersByDateFilter())
        {
        }

        public EbSoftSuppliers(IWebRequest server, IFilter filter)
        {
            _server = server;
            _filter = filter;
        }

        public IEntities<ISupplier> With(IFilter filter)
        {
            return new EbSoftSuppliers(_server, filter);
        }

        public async Task<IList<ISupplier>> ToListAsync()
        {
            var bySupplierName = new GroupedBy<string, JObject>(
                await _server
                    .WithFilter(_filter)
                    .ReadAsync<List<JObject>>(),
                (item) => item.Value<string>("nom")
            ).ToDictionary();
            return bySupplierName.Keys
                .Select(supplierName =>
                    new EbSoftSupplier(
                        _server,
                        supplierName,
                        bySupplierName[supplierName]
                    )
                ).ToList<ISupplier>();
        }
    }
}
