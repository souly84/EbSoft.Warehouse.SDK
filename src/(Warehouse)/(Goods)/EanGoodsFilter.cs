﻿using System;
using System.Collections.Generic;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class EanGoodsFilter : IFilter
    {
        private readonly string _goodEan;

        public EanGoodsFilter(string goodEan)
        {
            if (string.IsNullOrEmpty(goodEan))
            {
                throw new ArgumentNullException(nameof(goodEan));
            }
            _goodEan = goodEan;
        }

        public Dictionary<string, object> ToParams()
        {
            return new Dictionary<string, object>
            {
                { "filter", "getProduct" },
                { "ean", _goodEan },
            };
        }
    }
}
