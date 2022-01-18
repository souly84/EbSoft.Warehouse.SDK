using System;
using MediaPrint;
using Newtonsoft.Json.Linq;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public sealed class EbSoftReceptionGood : IReceptionGood, IEquatable<string>, IEquatable<int>
    {
        private readonly int _receptionId;
        private readonly JObject _ebSoftGood;
        private IGoodConfirmation _confirmation;

        public EbSoftReceptionGood(
            int receptionId,
            JObject ebSoftGood)
        {
            _receptionId = receptionId;
            _ebSoftGood = ebSoftGood;
        }

        private int Id => _ebSoftGood.Value<int>("id");

        private string Ean => _ebSoftGood.Value<string>("ean");

        private string Article => _ebSoftGood.Value<string>("article");

        private string ItemType => _ebSoftGood.Value<string>("itemType");

        private int ConfirmedQuantity => _ebSoftGood.Value<int>("qtin");

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, Quantity, ConfirmedQuantity));
       
        public int Quantity => _ebSoftGood.Value<int>("qt");

        public void PrintTo(IMedia media)
        {
            media
                .Put("Id", Id)
                .Put("ReceptionId", _receptionId)
                .Put("Article", Article)
                .Put("Ean", Ean)
                .Put("oa", _ebSoftGood.Value<string>("oa"))
                .Put("Quantity", _ebSoftGood.Value<int>("qt"))
                .Put("ItemType", ItemType)
                .Put("ConfirmedQuantity", ConfirmedQuantity);
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(obj, this)
                || obj is string data && Equals(data)
                || obj is int id && Equals(id);
        }

        public bool Equals(string data)
        {
            return data == Ean
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
