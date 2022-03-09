using System.Configuration;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftCompanyTests
    {
        private EbSoftCompany _ebSoftCompany = new EbSoftCompany(
             ConfigurationManager.AppSettings["companyUri"]
        );
        private readonly ITestOutputHelper _output;

        public EbSoftCompanyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Need to be implemented first")]
        public async Task SuccesfullLoginIntegration()
        {
            Assert.NotNull(
                await _ebSoftCompany.LoginAsync("", "")
            );
        }

        [Fact(Skip = "Need to be implemented first")]
        public Task UnsuccesfullLoginIntegration()
        {
            return Assert.ThrowsAsync<EbSoftInvalidLoginException>(
                () => _ebSoftCompany.LoginAsync("wrongEmail@gmail.com", "wrongPassword")
            );
        }
    }
}
