using WebRequest.Elegant;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public static class WebRequestExtensions
    {
        public static IWebRequest Logged(this IWebRequest request, ITestOutputHelper output)
        {
            return new LoggedWebRequest(request, output);
        }
    }
}
