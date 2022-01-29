using System;
using MediaPrint;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK
{
    public class NoWarehouseGood : IWarehouseGood
    {
        private readonly Exception _exception;

        public NoWarehouseGood()
            : this(new InvalidOperationException(
                "There is no goods defined for EbSoftWarehouse storage. " +
                "It means Warheouse.ByBarcodeAsync method was called but no information " +
                "about goods in this storage has been provided yet."))
        {
        }

        public NoWarehouseGood(Exception exception)
        {
            _exception = exception;
        }

        public int Quantity => throw _exception;

        public IStorages Storages => throw _exception;

        public IMovement Movement => throw _exception;

        public void PrintTo(IMedia media)
        {
            // Nothing to do
        }
    }
}
