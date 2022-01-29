using System.Threading.Tasks;
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
            Xunit.Assert.True(
                storage.Equals(storage)
            );
        }

        [Fact]
        public async Task ZeroQuantityForNonExistingGood()
        {
            Xunit.Assert.Equal(
                0,
                await new EbSoftStorage(
                    new JObject(),
                    new MockWarehouseGood("1", 1)
                ).QuantityForAsync(new MockWarehouseGood("2", 1))
            );
        }
    }
}
