using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.Tests
{
    public class EbSoftCompanyTests
    {
        private static string _ebSoftCompanyUri = ConfigurationManager.AppSettings["companyUri"];
        private const string _azureDevOpsSkipReason = "Not on corporate network";
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
            _ebSoftCompanyUri
        );
        private readonly ITestOutputHelper _output;

        public EbSoftCompanyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Need to be implemented first")]
        public async Task SuccesfullLogin()
        {
            Assert.NotNull(
                await _ebSoftCompany.LoginAsync("", "")
            );
        }

        [Fact(Skip = _azureDevOpsSkipReason)]
        public Task UnsuccesfullLogin()
        {
            return Assert.ThrowsAsync<EbSoftInvalidLoginException>(
                () => _ebSoftCompany.LoginAsync("wrongEmail@gmail.com", "wrongPassword")
            );
        }

        [Fact(Skip = _azureDevOpsSkipReason)]
        public async Task GetReceptions()
        {
            Assert.NotEmpty(
                await _ebSoftCompany.Warehouse.Receptions.ToListAsync()
            );
        }

        [Fact(Skip = _azureDevOpsSkipReason)]
        public async Task GetReceptionGoods()
        {
            var receptions = await _ebSoftCompany.Warehouse.Receptions.ToListAsync();
            Assert.NotEmpty(
                await receptions.First().Goods.ToListAsync()
            );
        }
    }
}
