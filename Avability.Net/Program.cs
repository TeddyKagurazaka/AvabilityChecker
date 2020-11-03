using System;

namespace Avability.Core
{
    class Program
    {
        static StoreInfo stores;
        static StockInfo stocks;

        static System.Timers.Timer updateTimer;
        static bool iPhone12Pro = true;
        static bool Beep = false;

        static System.IO.StreamWriter sWriter;

        static void Main(string[] args)
        {
            if(Beep)
                Console.Beep(800, 500);

            stores = new StoreInfo();
            stores.Update();

            stocks = new StockInfo(stores);
            stocks.Update(iPhone12Pro);

            sWriter = new System.IO.StreamWriter("Stocks.csv",true);
            sWriter.AutoFlush = true;

            updateTimer = new System.Timers.Timer(10000);
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.AutoReset = true;
            updateTimer.Start();

            try
            {
                while (true)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.F1:
                            Console.Clear();
                            iPhone12Pro = !iPhone12Pro;
                            Console.WriteLine("Switching to:" + (iPhone12Pro ? "12Pro" : "12"));
                            break;
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            stocks.PrintStocks();
                            break;
                        case ConsoleKey.Enter:
                            Console.Clear();
                            stocks.PrintStocks(true);
                            break;
                        case ConsoleKey.F2:
                            Beep = !Beep;
                            Console.WriteLine("Beep " + (Beep ? "Enabled" : "Disabled"));
                            break;
                        case ConsoleKey.Escape:
                            updateTimer.Stop();
                            sWriter.Flush();
                            sWriter.Close();
                            Environment.Exit(0);
                            break;
                    }
                }
            }
            catch
            {
                //因为各种各样的问题导致没办法Console.Readkey,改用最原始的
                Console.WriteLine("This Env doesn't support Console.ReadKey,falling back...");
            }

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "switch":
                        Console.Clear();
                        iPhone12Pro = !iPhone12Pro;
                        Console.WriteLine("Switching to:" + (iPhone12Pro ? "12Pro" : "12"));
                        break;
                    case "allstocks":
                        Console.Clear();
                        stocks.PrintStocks();
                        break;
                    case "stocks":
                        Console.Clear();
                        stocks.PrintStocks(true);
                        break;
                    case "beep":
                        Beep = !Beep;
                        Console.WriteLine("Beep " + (Beep ? "Enabled" : "Disabled"));
                        break;
                    case "esc":
                        updateTimer.Stop();
                        sWriter.Flush();
                        sWriter.Close();
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
            if (!stocks.Update(iPhone12Pro)) { Console.WriteLine("Update Fail"); updateTimer.Start(); return; }
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
                        //if(
                        //    (data.Key == "R502" || data.Key == "R580") //太古里or万象城
                        //    && Beep 
                        //    && (mdl == "MGLF3CH/A"  //白256
                        //    || mdl == "MGLA3CH/A"   //白128
                        //    || mdl == "MGLE3CH/A"   //黑256
                        //    || mdl == "MGL93CH/A")) //黑128

                        if(Beep)
                            Console.Beep(800, 500);

                        //将库存数据写入本地,用来统计库存信息
                        sWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6}",
                            DateTime.Now.ToString(),                        //当前时间
                            stocks.LastUpdate.ToLocalTime().ToString(),     //服务器库存时间(大约每30秒一次)
                            friendlyName,                                   //店名
                            data.Key,                                       //店ID(Rxxx)
                            mdl,                                            //型号
                            Translate(mdl),                                 //型号名(如 白色128G)
                            (iPhone12Pro ? "12 Pro" : "12")                 //12或者12Pro
                            );
                        sWriter.Flush();

                        Console.WriteLine("[{2}]***Stocks:{0} {1}***", friendlyName, Translate(mdl),DateTime.Now.ToShortTimeString());
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

        static string Translate(string model)
        {
            //这里只记录了Pro型号的颜色信息,其他返回原始型号
            switch (model)
            {
                case "MGLF3CH/A":
                    return "白256";
                case "MGLA3CH/A":
                    return "白128";
                case "MGLE3CH/A":
                    return "黑256";
                case "MGL93CH/A":
                    return "黑128";
                case "MGLC3CH/A":
                    return "金128";
                case "MGLG3CH/A":
                    return "金256";
                case "MGLD3CH/A":
                    return "蓝128";
                case "MGLH3CH/A":
                    return "蓝256";
                default:
                    return model;

            }
        }
    }
}
