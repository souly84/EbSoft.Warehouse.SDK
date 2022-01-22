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
            var extraConfirmedIdList = new List<string>();
            foreach (var confirmation in _goodsConfirmations)
            {
                if (confirmation.ConfirmedQuantity > 0)
                {
                    var goodData = confirmation.Good.ToDictionary();
                    var id = goodData.Value<string>("Id");
                    if (extraConfirmedIdList.Contains(id))
                    {
                        continue; // skip because its alreadybeen added as Extra confirmed good
                    }
                    if (confirmation.Good.IsExtraConfirmed)
                    {
                        extraConfirmedIdList.Add(id);
                    }

                    var jObject = new JObject(
                        new JProperty("id", id == "0" ? "" : id), // when id == "0" means unknown good
                        new JProperty("qty", confirmation.ConfirmedQuantity),
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
