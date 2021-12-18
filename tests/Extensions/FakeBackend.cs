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
            var root = new Uri("http://fake.company.com");
            var suppliersFilterDate = GlobalTestsParams.SuppliersDateTime.ToString("yyyy-MM-dd");
            Proxy = new ProxyHttpMessageHandler(
                new RoutedHttpMessageHandler(
                    new Route(new Dictionary<string, string>
                    {
                        { $"{root}?filter=setLinesCmr", "Success" },
                        { $"{root}?filter=assignProductTo&ean=4002516315155", "Success" }
                    }).With(
                        new Uri($"{root}?filter=getListCmr&date={suppliersFilterDate}"),
                        "./Data/Suppliers.json"
                    ).With(
                        new Uri($"{root}?filter=getCmrlines&id=5"),
                        "./Data/MieleReceptions.json"
                    ).With(
                        new Uri($"{root}?filter=getProduct&ean=4002516315155"),
                        "./Data/Product.json"
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
