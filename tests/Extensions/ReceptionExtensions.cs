using System.Threading.Tasks;
using Warehouse.Core;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class ReceptionExtensions
    {
        public static Task<T> FullyConfirmedAsync<T>(this T reception)
            where T : IReception
        {
            return new ConfirmedReception<T>(reception).ConfirmAsync();
        }
    }
}
