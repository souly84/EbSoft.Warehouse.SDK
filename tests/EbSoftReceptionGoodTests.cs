using System.Threading.Tasks;
using EbSoft.Warehouse.SDK.UnitTests.Extensions;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Xunit;
using Assert = Xunit.Assert;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionGoodTests
    {
        private string _confirmedGoodAsJson =
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

        private string _goodAsJson =
            @"{
                ""id"": ""45"",
                ""oa_dossier"": ""OA859840"",
                ""article"": ""GROHE 31566SD0"",
                ""qt"": ""1"",
                ""ean"": [ ""4005176473234"" ],
                ""qtin"": ""0"",
                ""error_code"": null,
                ""commentaire"": null,
                ""itemType"": ""electro"",
                ""qtscanned"": ""0""
             }";

        [Fact]
        public async Task InitiallyConfirmedReceptionGood()
        {
            Assert.True(
                await new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_confirmedGoodAsJson)
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
            reception = new StatefulReception(
               reception
                   .WithExtraConfirmed()
                   .WithoutInitiallyConfirmed(),
               new KeyValueStorage()
            );
            Assert.NotEqual(
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
                    JObject.Parse(_confirmedGoodAsJson)
                ).Equals("electro")
            );
        }

        [Fact]
        public void CanBeFoundByItemType()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_confirmedGoodAsJson)
                ).Equals("BEKO HCA63640BH")
            );
        }

        [Fact]
        public void CanBeFoundById()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_confirmedGoodAsJson)
                ).Equals(22)
            );
        }

        [Fact]
        public void EqualsToBarcode()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals("4005176473234")
            );
        }

        [Fact]
        public void EqualsToStateful()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals(
                    new StatefulReceptionGood(
                        new EbSoftReceptionGood(
                            1,
                            JObject.Parse(_goodAsJson)
                        ),
                        new KeyValueStorage(),
                        string.Empty
                    )
                )
            );
        }

        [Fact]
        public void GetHashCodeNotZero()
        {
            Assert.NotEqual(
                0,
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).GetHashCode()
            );
        }

        [Fact]
        public void NotEqualToNull()
        {
            Assert.False(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals(null)
            );
        }

        [Fact]
        public void EqualToUnknownGoodWithTheSameBarcode()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    "1111"
                ).Equals(new MockReceptionGood("1", 1, "1111", isUnknown: true))
            );
        }

        [Fact]
        public void EqualToMockGoodWithTheSameId()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(_goodAsJson)
                ).Equals(new MockReceptionGood("45", 5, "8690842130830"))
            );
        }

        [Fact]
        public void EmptyEanConsideredAsCorrupted()
        {
            Assert.True(
                new EbSoftReceptionGood(
                    1,
                    JObject.Parse(@"{
                        ""id"": ""40"",
                        ""oa"": ""OA847066"",
                        ""article"": ""MIELE WED035WPS"",
                        ""qt"": ""1"",
                        ""ean"": [],
                        ""qtin"": 0,
                        ""error_code"": null,
                        ""commentaire"": null,
                        ""itemType"": ""electro"",
                        ""qtscanned"": ""0""
                      }")
                ).ToDictionary().Value<bool>("Corrupted")
            );
        }
    }
}
