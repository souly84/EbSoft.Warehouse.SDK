using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Goods;
using Warehouse.Core.Goods.Storages;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGoodMovement : IMovement
    {
        private readonly IWebRequest _server;
        private readonly IGood _good;
        private readonly IStorage _fromStorage;

        public EbSoftGoodMovement(IWebRequest server, IGood good)
            : this(server, good, new IncorrectStorage())
        {
        }

        public EbSoftGoodMovement(IWebRequest server, IGood good, IStorage fromStorage)
        {
            _server = server;
            _good = good;
            _fromStorage = fromStorage;
        }

        public IMovement From(IStorage storage)
        {
            return new EbSoftGoodMovement(
                _server,
                _good,
                storage
            );
        }

        public Task MoveToAsync(IStorage storage, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}
