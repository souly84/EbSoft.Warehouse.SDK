using System;
using System.Threading.Tasks;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftCompany : ICompany
    {
        private IWebRequest _server;

        public EbSoftCompany(
            string companyUri
        ) : this(
                new WebRequest.Elegant.WebRequest(
                    companyUri
                )
            )
        {
        }


        public EbSoftCompany(IWebRequest server)
        {
            _server = server;
        }

        public ICustomers Customers => throw new NotImplementedException();

        public IUsers Users => throw new NotImplementedException();

        public IWarehouse Warehouse => new EbSoftWarehouse(_server);

        public Task<IUser> LoginAsync(string userName, string password)
        {
            throw new EbSoftInvalidLoginException("Not implemented");

        }
    }
}
