using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftWarehouseGoodTests
    {
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        );

        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GoodStorages()
        {
            var good = await _ebSoftCompany
                .Warehouse
                .Goods.For("")
                .FirstAsync();
            Xunit.Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }
    }
}
