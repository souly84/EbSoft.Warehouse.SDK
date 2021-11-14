using System;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public static class ReceptionExtensions
    {
        public static IReceptions For(this IReceptions receptions, DateTime dateTime)
        {
            return receptions.With(new ReceptionFilter(dateTime));
        }
    }
}
