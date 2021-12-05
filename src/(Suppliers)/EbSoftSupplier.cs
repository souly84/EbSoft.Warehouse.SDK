using System.Collections.Generic;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftSupplier : ISupplier
    {
        private readonly IWebRequest _server;
        private readonly string _supplierName;
        private readonly IList<JObject> _receptionsList;

        public EbSoftSupplier(
            IWebRequest server,
            string supplierName,
            IList<JObject> receptionsList)
        {
            _server = server;
            _supplierName = supplierName;
            _receptionsList = receptionsList;
        }

        public IEntities<IReception> Receptions => new EbSoftReceptions(
            _server,
            _receptionsList
        );

        public void PrintTo(IMedia media)
        {
            media
                .Put("Name", _supplierName)
                .Put("Receptions", Receptions);
        }
    }
}
