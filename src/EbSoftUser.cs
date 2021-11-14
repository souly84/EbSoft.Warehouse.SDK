using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftUser : IUser
    {
        private readonly IWebRequest _client;

        public EbSoftUser(IWebRequest client)
        {
            _client = client;
        }
    }
}
