using System.Configuration;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.Tests
{
    public class EbSoftCompanyTests
    {
        private const string _azureDevOpsSkipReason = null;
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
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
    }
}
