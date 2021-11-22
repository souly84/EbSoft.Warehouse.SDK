using System.Collections.Generic;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionConfirmationAsJsonBody : IJsonObject
    {
        private readonly IList<IGoodConfirmation> _confirmedGood;

        public ReceptionConfirmationAsJsonBody(IList<IGoodConfirmation> confirmedGoods)
        {
            _confirmedGood = confirmedGoods;
        }

        public string ToJson()
        {
            var array = new JArray();

            foreach (var confirmedGood in _confirmedGood)
            {
                var confirmationData = confirmedGood.ToDictionary();
                var confirmedQty = confirmationData.Value<int>("Confirmed");
                if (confirmedQty > 0)
                {
                    var goodData = confirmationData.Value<IGood>("Good").ToDictionary();
                    var jObject = new JObject(
                        new JProperty("id", goodData.Value<string>("Id")),
                        new JProperty("qty", confirmedQty.ToString()),
                        new JProperty("gtin", goodData.Value<string>("Ean")),
                        new JProperty("error_code", null)
                    );
                    array.Add(jObject);
                }
               
            }
            return array.ToString();
        }
    }
}
