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
                    array.Add(
                        new JObject(
                            new JProperty("id", Id(confirmation.Good)), 
                            new JProperty("qty", ConfirmedQty(confirmation)),
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

        private int ConfirmedQty(IGoodConfirmation confirmation)
        {
            if (ExtraConfirmedAsCombined(confirmation))
            {
                return confirmation.ConfirmedQuantity - confirmation.Good.Quantity;
            }
            return confirmation.ConfirmedQuantity;
        }

        private string Id(IReceptionGood receptionGood)
        {
            // when id == "0" means unknown good
            return receptionGood.Id == "0" || receptionGood.IsExtraConfirmed
                ? string.Empty
                : receptionGood.Id;
        }

        private bool ExtraConfirmedAsCombined(IGoodConfirmation confirmation)
        {
            return confirmation.Good.IsExtraConfirmed
                && confirmation.ConfirmedQuantity > confirmation.Good.Quantity;
        }
    }
}
