using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Xunit;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class EbSoftReceptionGoodTests
    {
        [Fact]
        public async Task InitiallyConfirmedReceptionGood()
        {
            Assert.True(
                await new EbSoftReceptionGood(
                    1,
                    JObject.Parse(
                        @"{
                            ""id"":""22"",
                            ""oa_dossier"":""OA843460"",
                            ""article"":""BEKO HCA63640BH"",
                            ""qt"":""5"",
                            ""ean"":""8690842130830"",
                            ""qtin"":""5"",
                            ""error_code"":null,
                            ""commentaire"":null,
                            ""itemType"":""electro"",
                            ""qtscanned"":""1""}"
                    )
                ).ConfirmedAsync()
            );
        }
    }
}
