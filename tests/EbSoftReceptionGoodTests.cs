using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionGoodTests
    {
        private string _goodAsJson =
            @"{
                ""id"":""22"",
                ""oa_dossier"":""OA843460"",
                ""article"":""BEKO HCA63640BH"",
                ""qt"":""5"",
                ""ean"":[ ""8690842130830"" ],
                ""qtin"":""5"",
                ""error_code"":null,
                ""commentaire"":null,
                ""itemType"":""electro"",
                ""qtscanned"":""5""
            }";

        [Fact]
        public async Task InitiallyConfirmedReceptionGood()
        {
            Assert.True(
                await new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).ConfirmedAsync()
            );
        }

        [Fact]
        public void UnknownGood()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    "UnknownBarcode"
                ).IsUnknown
            );
        }

        [Fact]
        public async Task UnknownGoodsDifferent()
        {
            
            var ebSoftServer = new EbSoftFakeServer();
            var reception = await new EbSoftCompanyReception(
                ebSoftServer.ToWebRequest()
                ).ReceptionAsync();
                
                Xunit.Assert.NotEqual(
                    reception.Goods.UnkownGood("1234"),
                    reception.Goods.UnkownGood("5678")
                );
        }

        [Fact]
        public void CanBeFoundByArticle()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals("electro")
            );
        }

        [Fact]
        public void CanBeFoundByItemType()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals("BEKO HCA63640BH")
            );
        }

        [Fact]
        public void CanBeFoundById()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals(22)
            );
        }
    }
}
