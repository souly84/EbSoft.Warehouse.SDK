using Newtonsoft.Json.Linq;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionGoodsTests
    {
        [Fact]
        public void UnknownGood()
        {
            Assert.Equal(
                new EbSoftReceptionGood(
                    1,
                    new JObject(
                      new JProperty("ean", new JArray("goodbarcode")),
                      new JProperty("qt", 1000)
                    ),
                    true
                ),
                new EbSoftReceptionGoods(
                    new WebRequest.Elegant.WebRequest("http://notexistin.com"),
                    1
                ).UnkownGood("goodbarcode")
            );
        }
    }
}
