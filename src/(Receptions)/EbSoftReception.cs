using System.Collections.Generic;
using System.Threading.Tasks;
using MediaPrint;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReception : IReception, IPrintable
    {
        private readonly IWebRequest _server;
        private readonly int _receptionId;
        private IReceptionGoods _goods;

        public EbSoftReception(
            IWebRequest server,
            int receptionId)
        {
            _server = server;
            _receptionId = receptionId;
        }

        public IReceptionGoods Goods => _goods ?? (_goods = new CachedReceptionGoods(
            new EbSoftReceptionGoods(
            _server,
            _receptionId)
        ));

        public void PrintTo(IMedia media)
        {
            media
                .Put("ReceptionId", _receptionId)
                .Put("Goods", Goods);
        }

        public Task ValidateAsync(IList<IGoodConfirmation> goodsToValidate)
        {
            return _server
                .WithMethod(System.Net.Http.HttpMethod.Post)
                .WithBody(
                    new Dictionary<string, IJsonObject>
                    {
                        { "filter", new SimpleString("setLinesCmr") },
                        { "json", new ReceptionConfirmationAsJsonBody(_receptionId, goodsToValidate) }
                    }
                )
                .EnsureSuccessAsync();
        }
    }
}
