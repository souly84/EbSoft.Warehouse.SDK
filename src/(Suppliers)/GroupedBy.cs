using System;
using System.Collections.Generic;

namespace EbSoft.Warehouse.SDK
{
    public class GroupedBy<Key, TObject>
    {
        private readonly List<TObject> _list;
        private readonly Func<TObject, Key> _keyPredicate;

        public GroupedBy(List<TObject> list, Func<TObject, Key> keyPredicate)
        {
            _list = list;
            _keyPredicate = keyPredicate;
        }

        public Dictionary<Key, List<TObject>> ToDictionary()
        {
            var result = new Dictionary<Key, List<TObject>>();
            foreach (var item in _list)
            {
                var key = _keyPredicate(item);
                if (!result.ContainsKey(key))
                {
                    result.Add(key, new List<TObject>());
                }
                result[key].Add(item);
            }

            return result;
        }
    }
}
