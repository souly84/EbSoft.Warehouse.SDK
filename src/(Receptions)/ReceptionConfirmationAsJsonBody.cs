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
        private readonly IList<IGoodConfirmation> _goodsConfirmations;

        public ReceptionConfirmationAsJsonBody(
            int receptionId,
            IList<IGoodConfirmation> goodsConfirmations)
        {
            _receptionId = receptionId;
            _goodsConfirmations = goodsConfirmations;
        }

        public string ToJson()
        {
            var array = new JArray();
            foreach (var confirmation in _goodsConfirmations)
            {
                if (confirmation.ConfirmedQuantity > 0)
                {
                    var goodData = confirmation.Good.ToDictionary();
                    var id = goodData.Value<string>("Id");
                    array.Add(
                        new JObject(
                            new JProperty("id", id == "0" ? "" : id), // when id == "0" means unknown good
                            new JProperty("qty", confirmation.ConfirmedQuantity),
                            new JProperty("gtin", goodData.Value<string>("Ean")),
                            new JProperty("error_code", null)
                        )
                    );
                }
               
            }
            return new JObject(
                new JProperty("cmrId", _receptionId),
                new JProperty("lines", array)
            ).ToString();
        }
    }
}
