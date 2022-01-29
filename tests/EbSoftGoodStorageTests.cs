using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftGoodStorageTests
    {
        [Fact]
        public void EqualsTheSame()
        {
            var storage = new EbSoftStorage(new JObject(), new MockWarehouseGood("1", 1));
            Xunit.Assert.Equal(
                storage,
                storage
            );
        }
    }
}
