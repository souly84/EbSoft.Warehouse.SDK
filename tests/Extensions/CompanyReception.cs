using System.Configuration;
using System.Threading.Tasks;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class EbSoftCompanyReception
    {
        private IEntities<ISupplier> _ebSoftSuppliers;
        
        public EbSoftCompanyReception()
            : this(ConfigurationManager.AppSettings["companyUri"])
        {
        }

        public EbSoftCompanyReception(string companyUri)
            : this(new WebRequest.Elegant.WebRequest(companyUri))
        {
        }

        public EbSoftCompanyReception(IWebRequest webRequest)
        {
            _ebSoftSuppliers = new EbSoftCompany(webRequest).Suppliers;
        }

        public async Task<IReception> ReceptionAsync(int supplierIndex = 0, int receptionIndex = 0)
        {
            var suppliers = await _ebSoftSuppliers
              .For(GlobalTestsParams.SuppliersDateTime)
              .ToListAsync();
            var receptions = await suppliers[supplierIndex]
                .Receptions
                .ToListAsync();
            return receptions[receptionIndex];
        }
    }
}
