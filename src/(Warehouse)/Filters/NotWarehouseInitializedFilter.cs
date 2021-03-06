using System;
using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.Warehouse
{
    public class NotWarehouseInitializedFilter : IFilter
    {
        public bool Matches(object entity)
        {
            throw InvalidWarehouseException();
        }

        public Dictionary<string, object> ToParams()
        {
            throw InvalidWarehouseException();
        }

        private Exception InvalidWarehouseException()
        {
            return new InvalidOperationException(
                "Warehouse goods can be received individually by Ean code only." +
                " Maybe you need to call warehouse.Goods.For(\"EanCode\") first."
            );
        }
    }
}
