using System;

namespace EbSoft.Warehouse.SDK
{
    public class EbSoftInvalidLoginException : Exception
    {

        public EbSoftInvalidLoginException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
