using System;
using System.Runtime.Serialization;

namespace EbSoft.Warehouse.SDK
{
    [Serializable]
    public class EbSoftInvalidLoginException : Exception
    {
        public EbSoftInvalidLoginException(string errorMessage) : base(errorMessage)
        {
        }

        protected EbSoftInvalidLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
