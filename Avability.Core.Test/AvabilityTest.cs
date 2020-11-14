using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Net;

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

        [TestMethod]
        public void StoreProbe()
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                try
                {
                    var req = WebRequest.CreateHttp(string.Format("https://reserve-prime.apple.com/CN/zh_CN/reserve/{0}/availability.json", i));
                    req.Timeout = 5000;
                    var resp = req.GetResponse() as HttpWebResponse;

                    if (resp.StatusCode == HttpStatusCode.OK)
                        Debug.Print(i.ToString());
                }
                catch
                {

                }
            }
        }
    }
}
