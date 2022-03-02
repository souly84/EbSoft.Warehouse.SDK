using System;
using System.Collections.Generic;
using Warehouse.Core;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class EbSoftFakeServer
    {
        public ProxyHttpMessageHandler Proxy { get; private set; }

        public IWebRequest ToWebRequest()
        {
            var root = new Uri("http://fake.company.com");
            var suppliersFilterDate = GlobalTestsParams.SuppliersDateTime.ToString("yyyy-MM-dd");
            return ToWebRequest(
                root,
                new Route(new Dictionary<string, string>
                    {
                        { $"{root}", "Success" },
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
                    ).With(
                        new Uri($"{root}?filter=getBoxes&ean=133037620160"),
                        "./Data/WarehouseStorage.json"
                    )
             );
        }

        public IWebRequest ToWebRequest(Uri root, IRoute route)
        {
            Proxy = new ProxyHttpMessageHandler(
                new RoutedHttpMessageHandler(
                    route
                )
            );
            return new WebRequest.Elegant.WebRequest(
                root,
                Proxy
            );
        }

        public ICompany Company()
        {
            return new EbSoftCompany(ToWebRequest());
        }

        public IWarehouse Warehouse()
        {
            return Company().Warehouse;
        }
    }
}
