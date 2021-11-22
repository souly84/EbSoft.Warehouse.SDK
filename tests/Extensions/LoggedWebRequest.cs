using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebRequest.Elegant;
using WebRequest.Elegant.Core;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.UnitTests
{
    public class LoggedWebRequest : IWebRequest
    {
        private readonly IWebRequest _origin;
        private readonly ITestOutputHelper _testOutputHelper;

        public LoggedWebRequest(IWebRequest origin, ITestOutputHelper testOutputHelper)
        {
            _origin = origin;
            _testOutputHelper = testOutputHelper;
        }

        public IToken Token => _origin.Token;

        public Uri Uri => _origin.Uri;

        public HttpMethod HttpMethod => _origin.HttpMethod;

        public IBodyContent Body => _origin.Body;

        public IWebRequest WithBody(IBodyContent postBody)
        {
            return new LoggedWebRequest(_origin.WithBody(postBody), _testOutputHelper);
        }

        public Task<HttpResponseMessage> GetResponseAsync()
        {
            _testOutputHelper.WriteLine(_origin.ToString());
            return _origin.GetResponseAsync();
        }

        public IWebRequest WithMethod(HttpMethod method)
        {
            return new LoggedWebRequest(
                _origin.WithMethod(method),
                _testOutputHelper
            );
        }

        public IWebRequest WithPath(Uri uri)
        {
            return new LoggedWebRequest(
                _origin.WithPath(uri),
                _testOutputHelper
            );
        }

        public IWebRequest WithQueryParams(Dictionary<string, string> parameters)
        {
            return new LoggedWebRequest(
                _origin.WithQueryParams(parameters),
                _testOutputHelper
            );
        }
    }
}
