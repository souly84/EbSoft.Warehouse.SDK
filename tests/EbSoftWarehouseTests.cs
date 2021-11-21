//using System;
//using System.Configuration;
//using System.Linq;
//using System.Threading.Tasks;
//using Warehouse.Core;
//using Xunit;
//using Xunit.Abstractions;

//namespace EbSoft.Warehouse.SDK.UnitTests
//{
//    public class EbSoftWarehouseTests
//    {
//        private readonly ITestOutputHelper _output;
//        private IWarehouse _ebSoftWarehouse;

//        public EbSoftWarehouseTests(ITestOutputHelper output)
//        {
//            _output = output;
//            _ebSoftWarehouse = new EbSoftCompany(
//                new WebRequest.Elegant.WebRequest(
//                    ConfigurationManager.AppSettings["companyUri"]
//                ).Logged(output)
//            ).Warehouse;
//        }

//        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
//        public async Task GetReceptions()
//        {
//            Assert.NotEmpty(
//                await _ebSoftWarehouse
//                    .Receptions.For(GlobalTestsParams.SuppliersDateTime)
//                    .ToListAsync()
//            );
//        }

//        [Fact(Skip = GlobalTestsParams.AzureDevOpsSkipReason)]
//        public async Task GetReceptionGoods()
//        {
//            var receptions = await _ebSoftWarehouse
//                .Receptions.For(GlobalTestsParams.SuppliersDateTime)
//                .ToListAsync();
//            Assert.NotEmpty(
//                await receptions.First().Goods.ToListAsync()
//            );
//        }
//    }
//}
