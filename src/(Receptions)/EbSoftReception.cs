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
        private readonly string _receptionId;
        private IEntities<IReceptionGood> _goods;

        public EbSoftReception(
            IWebRequest server,
            string receptionId)
        {
            _server = server;
            _receptionId = receptionId;
        }

        public IEntities<IReceptionGood> Goods => _goods ?? (_goods = new EbSoftReceptionGoods(
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
                .WithQueryParams(
                    new Dictionary<string, string>
                    {
                        { "filter", "getCmrlines" },
                    }
                )
                .WithMethod(System.Net.Http.HttpMethod.Post)
                .WithBody(new ReceptionConfirmationAsJsonBody(goodsToValidate))
                .EnsureSuccessAsync();
        }
    }
}
