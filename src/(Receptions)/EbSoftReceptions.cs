using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Core;
using WebRequest.Elegant;
using EbSoft.Warehouse.SDK.Extensions;
using Newtonsoft.Json.Linq;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptions : IReceptions
    {
        private readonly IWebRequest _server;
        private readonly IFilter _filter;

        public EbSoftReceptions(IWebRequest server)
            : this(server, new ReceptionFilter())
        {
        }

        public EbSoftReceptions(IWebRequest server, IFilter filter)
        {
            _server= server;
            _filter = filter;
        }

        public IReceptions With(IFilter filter)
        {
            return new EbSoftReceptions(_server, filter);
        }

        public Task<IList<IReception>> ToListAsync()
        {
            return _server
                .WithFilter(_filter)
                .SelectAsync(ToReception);
        }

        private IReception ToReception(JObject reception)
        {
            return new EbSoftReception(_server, reception);
        }
    }
}
