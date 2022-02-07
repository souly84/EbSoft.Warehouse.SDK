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
            string barcode)
            : this(
                  receptionId,
                  new JObject(
                      new JProperty("ean", new JArray(barcode)),
                      new JProperty("qt", 1000)
                  ),
                  true
              )
        {
        }

        public EbSoftReceptionGood(
            int receptionId,
            JObject ebSoftGood,
            bool isUnknown = false)
        {
            _receptionId = receptionId;
            _ebSoftGood = ebSoftGood;
            IsUnknown = isUnknown;
        }

        private int Id => _ebSoftGood.Value<int>("id");

        string IReceptionGood.Id => Id.ToString();

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

        private int ConfirmedQuantity => _ebSoftGood.Value<int>("qtscanned");

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, Quantity, ConfirmedQuantity));

        public int Quantity => _ebSoftGood.Value<int>("qt");

        public bool IsUnknown { get; }

        public bool IsExtraConfirmed => false;

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
                || SameReceptionGood(obj)
                || (obj is string data && Equals(data))
                || (obj is int id && Equals(id));
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

        private bool SameReceptionGood(object obj)
        {
            if (obj is EbSoftReceptionGood ebSoftReceptionGood)
            {
                if (ebSoftReceptionGood.IsUnknown)
                {
                    return Eans.Any(ean => ebSoftReceptionGood.Eans.Contains(ean));
                }
                return Id == ebSoftReceptionGood.Id;
            }
            else if (obj is IReceptionGood good && good != null)
            {
                return good.Equals(this);
            }
            return false;
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
