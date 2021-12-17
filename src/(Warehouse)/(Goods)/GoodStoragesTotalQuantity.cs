using Newtonsoft.Json.Linq;

namespace EbSoft.Warehouse.SDK
{
    public class GoodStoragesTotalQuantity
    {
        private readonly JArray _locations;

        public GoodStoragesTotalQuantity(JObject warehouseGoodData)
            : this(warehouseGoodData.Value<JArray>("locations"))
        {
        }

        public GoodStoragesTotalQuantity(JArray locations)
        {
            _locations = locations;
        }

        public int ToInt()
        {
            var total = 0;
            foreach (var location in _locations)
            {
                total += location.Value<int>("quantity");
            }
            return total;
        }
    }
}
