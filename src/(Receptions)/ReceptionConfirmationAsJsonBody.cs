using System.Collections.Generic;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class ReceptionConfirmationAsJsonBody : IJsonObject
    {
        private readonly IList<IGoodConfirmation> _goodsToValidate;

        public ReceptionConfirmationAsJsonBody(IList<IGoodConfirmation> goodsToValidate)
        {
            _goodsToValidate = goodsToValidate;
        }

        public string ToJson()
        {
            var array = new JArray();
            foreach (var good in _goodsToValidate)
            {
                array.Add(good.ToJson().ToString());
            }
            return array.ToString();
        }
    }
}
