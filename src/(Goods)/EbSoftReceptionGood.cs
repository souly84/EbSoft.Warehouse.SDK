using System;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;
using Warehouse.Core.Goods;
using WebRequest.Elegant;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftReceptionGood : IGood, IEquatable<string>, IEquatable<int>
    {
        private readonly IWebRequest _server;
        private readonly JObject _ebSoftGood;
        private IGoodConfirmation _confirmation;
        private IEntities<IStorage> _storages;

        public EbSoftReceptionGood(
            IWebRequest server,
            JObject ebSoftGood)
        {
            _server = server;
            _ebSoftGood = ebSoftGood;
        }

        private int Id => _ebSoftGood.Value<int>("id");

        private string Barcode => _ebSoftGood.Value<string>("ean");

        private string Article => _ebSoftGood.Value<string>("article");

        private string ItemType => _ebSoftGood.Value<string>("itemType");

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, Quantity));
       
        public int Quantity => _ebSoftGood.Value<int>("qt");

        public IEntities<IStorage> Storages => _storages ?? (_storages = new EbSoftGoodStorages(_server, this));

        public IMovement Movement => new EbSoftGoodMovement(_server, this);

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

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(obj, this)
                || obj is string data && Equals(data)
                || obj is int id && Equals(id);
        }

        public bool Equals(string data)
        {
            return data == Barcode
                || data == Article
                || data == ItemType;
        }

        public bool Equals(int id)
        {
            return id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
