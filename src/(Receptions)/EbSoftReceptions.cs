using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core;
using WebRequest.Elegant;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptions : IEntities<IReception>
    {
        private readonly IWebRequest _server;
        private readonly IList<JObject> _receptions;

        public EbSoftReceptions(IWebRequest server, IList<JObject> receptions)
        {
            _server = server;
            _receptions = receptions;
        }

        public IEntities<IReception> With(IFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IReception>> ToListAsync()
        {
            return Task.FromResult<IList<IReception>>(
                _receptions
                    .Select(reception => new EbSoftReception(_server, reception.Value<int>("id")))
                    .ToList<IReception>()
            );
        }
    }
}
