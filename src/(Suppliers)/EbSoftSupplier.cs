using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftSupplier : ISupplier
    {
        private readonly IWebRequest _server;
        private readonly IList<JObject> _receptionsList;

        public EbSoftSupplier(
            IWebRequest server,
            IList<JObject> receptionsList)
        {
            _server = server;
            _receptionsList = receptionsList;
        }

        public IEntities<IReception> Receptions => new EbSoftReceptions(
            _server,
            _receptionsList
        );
    }
}
