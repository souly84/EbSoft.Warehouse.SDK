using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<string> _eans;

        public EbSoftReceptionGood(
            int receptionId,
            JObject ebSoftGood)
        {
            _receptionId = receptionId;
            _ebSoftGood = ebSoftGood;
           
        }

        private int Id => _ebSoftGood.Value<int>("id");
        private string _lastTimeFoundByEan;

        private List<string> Eans
        {
            get
            {
                if (_eans == null)
                {
                    _eans = _ebSoftGood
                        .Value<JArray>("ean")
                        .ToObject<List<string>>();
                }
                return _eans;
            }
        }

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
                .Put("Ean", UsedEan())
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
            if (Eans.Contains(data))
            {
                _lastTimeFoundByEan = data;
                return true;
            }
            return data == Article
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

        private string UsedEan()
        {
            if (!string.IsNullOrEmpty(_lastTimeFoundByEan))
            {
                return _lastTimeFoundByEan;
            }
            return Eans.First(); // should be at least one otherwise its an issue
        }
    }
}
