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
        private readonly string _receptionId;
        private readonly JObject _ebSoftGood;
        private IGoodConfirmation _confirmation;
        private IEntities<IStorage> _storages;

        public EbSoftReceptionGood(
            IWebRequest server,
            string receptionId,
            JObject ebSoftGood)
        {
            _server = server;
            _receptionId = receptionId;
            _ebSoftGood = ebSoftGood;
        }

        private int Id => _ebSoftGood.Value<int>("id");

        private string Ean => _ebSoftGood.Value<string>("ean");

        private string Article => _ebSoftGood.Value<string>("article");

        private string ItemType => _ebSoftGood.Value<string>("itemType");

        public IGoodConfirmation Confirmation => _confirmation ?? (_confirmation = new GoodConfirmation(this, Quantity));
       
        public int Quantity => _ebSoftGood.Value<int>("qt");

        public IEntities<IStorage> Storages
        {
            get
            {
                if (_storages == null)
                {
                    if (string.IsNullOrEmpty(Ean))
                    {
                        throw new InvalidOperationException(
                            $"Good does not contain Ean\n{this.ToJson()}"
                        );
                    }

                    _storages = new EbSoftGoodStorages(_server, Ean);
                }
                return _storages;
            }
        }

        public IMovement Movement => new EbSoftGoodMovement(_server, this);

        public void PrintTo(IMedia media)
        {
            media
                .Put("Id", Id)
                .Put("ReceptionId", _receptionId)
                .Put("Article", Article)
                .Put("Ean", Ean)
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
