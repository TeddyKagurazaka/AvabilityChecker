using System;

namespace Avability.Core
{
    class Program
    {
        static StoreInfo stores;
        static StockInfo stocks;

        static System.Timers.Timer updateTimer;
        static bool iPhone12Pro = true;

        static void Main(string[] args)
        {
            stores = new StoreInfo();
            stores.Update();

            stocks = new StockInfo(stores);
            stocks.Update(iPhone12Pro);

            updateTimer = new System.Timers.Timer(10000);
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.AutoReset = true;
            updateTimer.Start();

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.F1:
                        Console.Clear();
                        Console.WriteLine("Switching to:" + (iPhone12Pro ? "12" : "12Pro"));
                        iPhone12Pro = !iPhone12Pro;
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        stocks.PrintStocks();
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        stocks.PrintStocks(true);
                        break;
                    case ConsoleKey.Escape:
                        updateTimer.Stop();
                        Environment.Exit(0);
                        break;
                }
            }
        }

        //https://reserve-prime.apple.com/CN/zh_CN/reserve/F?color=%E7%BA%A2%E8%89%B2&capacity=64GB&quantity=1&anchor-store=R705&store=R705&partNumber=MGGP3CH%2FA&channel=&sourceID=&iUID=&iuToken=&iUP=N&appleCare=&rv=&path=&plan=unlocked

        private static void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateTimer.Stop();
            Console.Write("Updating Stock...");
            if (!stocks.Update(iPhone12Pro)) { Console.WriteLine("Update Fail"); return; }
            else Console.WriteLine("Success");

            var available = stocks.FindStocks();
            if (available.Count > 0)
            {
                foreach (var data in available)
                {
                    var friendlyName = data.Key;
                    if (stores != null)
                    {
                        var storeData = stores.FindStore(data.Key);
                        if (storeData != null)
                        {
                            friendlyName = storeData.storeName;
                        }
                    }

                    foreach (var mdl in data.Value)
                    {
                        Console.WriteLine("***Stocks:{0} {1}***", friendlyName, mdl);
                        Console.WriteLine(string.Format("https://reserve-prime.apple.com/CN/zh_CN/reserve/{2}?" +
                                                        "anchor-store={0}" +
                                                        "&store={0}" +
                                                        "&partNumber={1}" +
                                                        "&channel=&sourceID=&iUID=&iuToken=&iUP=N&appleCare=&rv=&path=&plan=unlocked", data.Key, mdl,(iPhone12Pro ? "A" :"F")));
                    }
                }
            }
            updateTimer.Start();
        }
    }
}
