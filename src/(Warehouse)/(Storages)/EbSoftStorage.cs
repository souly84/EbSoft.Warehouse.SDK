using System;
using System.Threading.Tasks;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftStorage : IStorage
    {
        public EbSoftStorage(
            IWebRequest server,
            JObject storage)
        {
        }

        public IEntities<IGood> Goods => throw new NotImplementedException();

        public Task DecreaseAsync(IGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseAsync(IGood good, int quantity)
        {
            throw new NotImplementedException();
        }

        public void PrintTo(IMedia media)
        {
            throw new NotImplementedException();
        }
    }
}
