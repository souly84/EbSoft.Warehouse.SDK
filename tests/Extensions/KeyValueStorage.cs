using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Warehouse.Core.Plugins;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class KeyValueStorage : IKeyValueStorage
    {
        private Dictionary<string, string> _simpleStorage = new Dictionary<string, string>();

        public KeyValueStorage()
        {
        }

        public IList<string> Keys => _simpleStorage.Keys.ToList();

        public bool Contains(string key)
        {
            return _simpleStorage.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            if (typeof(T) == typeof(JObject))
            {
                string json = null;
                if (_simpleStorage.ContainsKey(key))
                {
                    json = _simpleStorage[key];
                }

                if (string.IsNullOrEmpty(json))
                {
                    return (T)(object)new JObject();
                }
                return (T)(object)JObject.Parse(json);
            }
            if (_simpleStorage.ContainsKey(key))
            {
                return (T)Convert.ChangeType(_simpleStorage[key], typeof(T));
            }
            return default;
        }

        public void Remove(string key)
        {
            _simpleStorage.Remove(key);
        }

        public void Set<T>(string key, T @object)
        {
            if (typeof(T) == typeof(JObject))
            {
                _simpleStorage[key] = @object.ToString();
            }
            else
            {
                _simpleStorage[key] = Convert.ToString(@object);
            }
        }
    }
}
