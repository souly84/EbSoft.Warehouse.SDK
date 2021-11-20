using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftGood : IGood, IPrintable
    {
        private readonly IWebRequest _server;
        private readonly JObject _ebSoftGood;
        private IGoodConfirmation _confirmation;

        public EbSoftGood(
            IWebRequest server,
            JObject ebSoftGood)
        {
            _server = server;
            _ebSoftGood = ebSoftGood;
        }

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, Quantity));

        private int Id => _ebSoftGood.Value<int>("id");

        private string Barcode => _ebSoftGood.Value<string>("ean");

        private string Article => _ebSoftGood.Value<string>("article");

        private string ItemType => _ebSoftGood.Value<string>("itemType");

        private int Quantity => _ebSoftGood.Value<int>("qt");

        public override bool Equals(object obj)
        {
            return TheSameId(obj)
                || TheSameBarcode(obj)
                || TheSameArticle(obj)
                || TheSameType(obj);
        }

        public override int GetHashCode()
        {
            return _ebSoftGood.Value<int>("id");
        }

        public void PrintTo(IMedia media)
        {
            media
                .Put("Id", Id)
                .Put("Article", Article)
                .Put("Ean", Barcode)
                .Put("oa", _ebSoftGood.Value<string>("oa"))
                .Put("Quantity", _ebSoftGood.Value<int>("qt"))
                .Put("ItemType", ItemType);
        }

        private bool TheSameBarcode(object obj)
        {
            return obj is string barcode && barcode == Barcode;
        }

        private bool TheSameType(object obj)
        {
            return obj is string itemType && itemType == ItemType;
        }

        private bool TheSameArticle(object obj)
        {
            return obj is string article && article == Article;
        }

        private bool TheSameId(object obj)
        {
            return obj is int id && id == Id;
        }
    }
}
