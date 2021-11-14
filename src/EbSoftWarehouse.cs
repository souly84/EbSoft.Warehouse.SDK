using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftWarehouse : IWarehouse
    {
        private readonly IWebRequest _server;

        public EbSoftWarehouse(IWebRequest server)
        {
            _server = server;
        }

        public IReceptions Receptions => new EbSoftReceptions(_server);
    }
}
