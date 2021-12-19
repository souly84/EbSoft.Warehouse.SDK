using System.Collections.Generic;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionConfirmationAsJsonBody : IJsonObject
    {
        private readonly int _receptionId;
        private readonly IList<IGoodConfirmation> _confirmedGood;

        public ReceptionConfirmationAsJsonBody(
            int receptionId,
            IList<IGoodConfirmation> confirmedGoods)
        {
            _receptionId = receptionId;
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
                    var goodData = confirmationData.Value<IReceptionGood>("Good").ToDictionary();
                    var jObject = new JObject(
                        new JProperty("id", goodData.Value<string>("Id")),
                        new JProperty("qty", confirmedQty.ToString()),
                        new JProperty("gtin", goodData.Value<string>("Ean")),
                        new JProperty("error_code", null)
                    );
                    array.Add(jObject);
                }
               
            }
            return new JObject(
                new JProperty("cmrId", _receptionId),
                new JProperty("lines", array)
            ).ToString();
        }
    }
}
