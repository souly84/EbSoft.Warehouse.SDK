using System;
using System.Collections.Generic;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class FakeBackend
    {
        public ProxyHttpMessageHandler Proxy { get; private set; }

        public IWebRequest ToWebRequest()
        {
            var root = "http://fake.company.com";
            var suppliersFilterDate = GlobalTestsParams.SuppliersDateTime.ToString("yyyy-MM-dd");
            Proxy = new ProxyHttpMessageHandler(
                new RoutedHttpMessageHandler(
                    new Route(new Dictionary<string, string>
                    {
                        { $"{root}/reception/validation", "Success" }
                    }).With(
                        new Uri($"{root}?filter=getListCmr&date={suppliersFilterDate}"),
                        "./Data/Suppliers.json"
                    ).With(
                        new Uri($"{root}?filter=getCmrlines&id=5"),
                        "./Data/MieleReceptions.json"
                    )
                )
            );
            return new WebRequest.Elegant.WebRequest(
                root,
                Proxy
            );
        }
    }
}
