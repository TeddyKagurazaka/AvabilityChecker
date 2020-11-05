using System;
using System.Collections.Generic;

namespace Avability.Core
{
    class Program
    {
        public enum Model
        {
            iPhone12Pro,
            iPhone12ProMax_iUP,
            iPhone12ProMax,
            iPhone12,
            iPhone12mini_iUP,
            iphone12mini
        }

        static StoreInfo stores;
        static StockInfo stocks;

        static System.Timers.Timer updateTimer;
        static Model mdl = Model.iPhone12Pro;

        static bool Roulette = false;
        static bool Beep = false;

        static System.IO.StreamWriter sWriter;

        static void Main(string[] args)
        {
            if(Beep)
                Console.Beep(800, 500);

            stores = new StoreInfo();
            stores.Update();

            stocks = new StockInfo(stores);
            stocks.Update(ModelToChannel(mdl));

            sWriter = new System.IO.StreamWriter("Stocks.csv",true);
            sWriter.AutoFlush = true;

            updateTimer = new System.Timers.Timer(10000);
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.AutoReset = true;
            updateTimer.Start();

            Console.WriteLine("Avability Online.");

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.F1:
                        Console.Clear();
                        updateTimer.Stop();
                        ChangeChannel();
                        updateTimer.Start();
                        break;

                    case ConsoleKey.F3:
                        Console.Clear();
                        updateTimer.Stop();
                        Roulette = !Roulette;
                        if (Roulette)
                        {
                            updateTimer.Interval = 5000;
                            Console.WriteLine("Roulette Mode,Scanning All Models.");
                        }
                        else
                        {
                            updateTimer.Interval = 10000;
                            Console.WriteLine("Normal Mode,Scanning " + mdl.ToString());
                        }
                        updateTimer.Start();
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
        static void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateTimer.Stop();
            Console.Write("Updating Stock...");

            if (!Roulette)
            {
                UpdateStocks(mdl);
            }
            else
            {
                foreach (var chns in new List<Model> { Model.iPhone12Pro, Model.iPhone12ProMax, Model.iPhone12ProMax_iUP,Model.iPhone12, Model.iphone12mini, Model.iPhone12mini_iUP })
                {
                    UpdateStocks(chns);
                    System.Threading.Thread.Sleep(1000);
                }
            }

            updateTimer.Start();
            return;
        }
        static void UpdateStocks(Model mdl)
        {
            if (!stocks.Update(ModelToChannel(mdl)))
            {
                Console.WriteLine(mdl.ToString() + " update fail.");
                return;
            }
            else
                Console.WriteLine(mdl.ToString() + " success");

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

                    foreach (var mdldata in data.Value)
                    {
                        if (Beep)
                            Console.Beep(800, 500);

                        //将库存数据写入本地,用来统计库存信息
                        sWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6}",
                            DateTime.Now.ToString(),                        //当前时间
                            stocks.LastUpdate.ToLocalTime().ToString(),     //服务器库存时间(大约每30秒一次)
                            friendlyName,                                   //店名
                            data.Key,                                       //店ID(Rxxx)
                            mdldata,                                            //型号
                            Translate(mdldata),                                 //型号名(如 白色128G)
                            ModelToChannel(mdl)                 //渠道
                            );
                        sWriter.Flush();

                        Console.WriteLine("[{2}]***Stocks:{0} {1} {2}***", friendlyName, Translate(mdldata), DateTime.Now.ToShortTimeString(), mdl.ToString());
                    }
                }
            }

            return;
        }
        static void ChangeChannel()
        {
            switch (mdl)
            {
                case Model.iPhone12Pro:
                    mdl = Model.iPhone12ProMax;
                    Console.WriteLine("Changing to iPhone 12 Pro Max");
                    break;
                case Model.iPhone12ProMax:
                    mdl = Model.iPhone12ProMax_iUP;
                    Console.WriteLine("Changing to iPhone 12 Pro Max(iUP Program)");
                    break;
                case Model.iPhone12ProMax_iUP:
                    mdl = Model.iPhone12;
                    Console.WriteLine("Changing to iPhone 12");
                    break;
                case Model.iPhone12:
                    mdl = Model.iphone12mini;
                    Console.WriteLine("Changing to iPhone 12 mini");
                    break;
                case Model.iphone12mini:
                    mdl = Model.iPhone12mini_iUP;
                    Console.WriteLine("Changing to iPhone 12 mini(iUP Program)");
                    break;
                case Model.iPhone12mini_iUP:
                    mdl = Model.iPhone12Pro;
                    Console.WriteLine("Changing to iPhone 12 Pro");
                    break;

            }
        }
        static string ModelToChannel(Model mdl)
        {
            switch (mdl)
            {
                case Model.iPhone12Pro:
                    return "A";
                case Model.iPhone12ProMax:
                    return "B";
                case Model.iPhone12ProMax_iUP:
                    return "C";
                case Model.iPhone12:
                    return "F";
                case Model.iphone12mini:
                    return "G";
                case Model.iPhone12mini_iUP:
                    return "H";
                default: return "A";
            }
        }
        static string Translate(string model)
        {
            //这里只记录了Pro型号的颜色信息,其他返回原始型号
            switch (model)
            {
                case "MGLF3CH/A":
                    return "12Pro 白256";
                case "MGLA3CH/A":
                    return "12Pro 白128";
                case "MGLE3CH/A":
                    return "12Pro 黑256";
                case "MGL93CH/A":
                    return "12Pro 黑128";
                case "MGLC3CH/A":
                    return "12Pro 金128";
                case "MGLG3CH/A":
                    return "12Pro 金256";
                case "MGLD3CH/A":
                    return "12Pro 蓝128";
                case "MGLH3CH/A":
                    return "12Pro 蓝256";
                default:
                    return model;

            }
        }
    }
}
