using System.Collections.Generic;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReception : IReception, IPrintable
    {
        private readonly IWebRequest _server;
        private readonly string _receptionId;
        private IEntities<IGood> _goods;

        public EbSoftReception(
            IWebRequest server,
            string receptionId)
        {
            _server = server;
            _receptionId = receptionId;
        }

        public IEntities<IGood> Goods => _goods ?? (_goods = new EbSoftStockGoods(
            _server,
            _receptionId).Cached()
        );

        public void PrintTo(IMedia media)
        {
            media
                .Put("ReceptionId", _receptionId)
                .Put("Goods", Goods);
        }

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            return _server
                .WithRelativePath("/reception/validation")
                .WithBody(new ReceptionConfirmationAsJsonBody(goodsToValidate))
                .EnsureSuccessAsync();
        }
    }
}
