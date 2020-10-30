using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avability.Core.Test
{
    [TestClass]
    public class AvabilityTest
    {
        [TestMethod]
        public void StockTest()
        {
            var stocks = new StockInfo();
            Assert.IsTrue(stocks.Update());
        }

        [TestMethod]
        public void StoreTest()
        {
            var stores = new StoreInfo();
            Assert.IsTrue(stores.Update());
        }
    }
}
