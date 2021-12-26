using System.Configuration;
using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Warehouse.Core;
using WebRequest.Elegant.Fakes;
using Xunit;
using Assert = EbSoft.Warehouse.SDK.UnitTests.Extensions.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftGoodStoragesTests
    {
        [Fact]
        public async Task PutAwayStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                1,
                (await good.Storages.PutAway.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task RaceStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                1,
                (await good.Storages.Race.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task ReserveStorages()
        {
            var good = await new EbSoftCompany(
                new FakeBackend().ToWebRequest()
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            Assert.Equal(
                0,
                (await good.Storages.Reserve.ToListAsync()).Count
            );
        }

        [Fact]
        public async Task ReserveStoragesOnTestEnv()
        {
            var proxy = new ProxyHttpMessageHandler();

            var good = await new EbSoftCompany(
                new WebRequest.Elegant.WebRequest(
                    ConfigurationManager.AppSettings["companyUri"], proxy)
            ).Warehouse.Goods.For("4002516315155")
             .FirstAsync();
            var check = await good.Storages.PutAway.ToListAsync();
            Assert.Equal(
                0,
                (await good.Storages.Reserve.ToListAsync()).Count
            );
        }
    }
}
