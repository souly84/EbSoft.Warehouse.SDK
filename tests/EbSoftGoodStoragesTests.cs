using System;
using System.Linq;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftGoodStoragesTests
    {
        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
        public async Task GoodStorages()
        {
            var reception = await new EbSoftCompanyReception(new DateTime(2021, 10, 30)).ReceptionAsync();
            var good = await reception.Goods.FirstAsync(async g =>
            {
                return (await g.Storages.ToListAsync()).Any();
            });
            Xunit.Assert.NotEmpty(
                await good.Storages.ToListAsync()
            );
        }
    }
}
