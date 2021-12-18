using System;
using System.Configuration;
using System.Threading.Tasks;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class EbSoftCompanyReception
    {
        private readonly DateTime _suppliersDate;
        private IEntities<ISupplier> _ebSoftSuppliers;

        public EbSoftCompanyReception()
            : this(GlobalTestsParams.SuppliersDateTime)
        {
        }

        public EbSoftCompanyReception(DateTime suppliersDate)
            : this(ConfigurationManager.AppSettings["companyUri"], suppliersDate)
        {
        }

        public EbSoftCompanyReception(string companyUri, DateTime suppliersDate)
            : this(
                  new WebRequest.Elegant.WebRequest(companyUri),
                  suppliersDate
              )
        {
        }

        public EbSoftCompanyReception(IWebRequest webRequest)
            : this(webRequest, GlobalTestsParams.SuppliersDateTime)
        {
        }

        public EbSoftCompanyReception(
            IWebRequest webRequest,
            DateTime suppliersDate)
        {
            _ebSoftSuppliers = new EbSoftCompany(webRequest).Suppliers;
            _suppliersDate = suppliersDate;
        }

        public async Task<IReception> ReceptionAsync(int supplierIndex = 0, int receptionIndex = 0)
        {
            var suppliers = await _ebSoftSuppliers
              .For(_suppliersDate)
              .ToListAsync();
            var receptions = await suppliers[supplierIndex]
                .Receptions
                .ToListAsync();
            return receptions[receptionIndex];
        }
    }
}
