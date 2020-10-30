﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Avability.Core
{
    public class StockInfo
    {
        public class ModelTemp
        {
            public string ModelID = "";
            public StockTemp Stock = new StockTemp();
        }

        public class StockTemp{
            public bool contract = false;
            public bool unlocked = false;
        }

        public Dictionary<string, List<ModelTemp>> StoreStocks;
        public StoreInfo storeInfo;

        public StockInfo()
        {
            StoreStocks = new Dictionary<string, List<ModelTemp>>();
        }

        public StockInfo(StoreInfo Stores) : this()
        {
            storeInfo = Stores;
        }

        public bool Update()
        {
            var data = Internals.Request("https://reserve-prime.apple.com/CN/zh_CN/reserve/A/availability.json");
            if (string.IsNullOrEmpty(data)) return false;

            var contents = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            var stores = JsonConvert.DeserializeObject<Dictionary<string, object>>(contents["stores"].ToString());

            foreach(var store in stores)
            {
                var storeID = store.Key;
                var models = JsonConvert.DeserializeObject<Dictionary<string, object>>(store.Value.ToString());

                var LocalStorage = new List<ModelTemp>();
                foreach(var model in models)
                {
                    var modelID = model.Key;
                    var avail = JsonConvert.DeserializeObject<Dictionary<string, StockTemp>>(model.Value.ToString());

                    var newModelTemp = new ModelTemp();
                    newModelTemp.ModelID = modelID;
                    newModelTemp.Stock = avail["availability"];
                    LocalStorage.Add(newModelTemp);
                }
                StoreStocks.Add(storeID, LocalStorage);
            }

            Console.WriteLine("Stocks Loaded:" + StoreStocks.Count);
            return true;
        }

        public Dictionary<string,List<string>> FindStocks()
        {
            var output = new Dictionary<string, List<string>>();
            foreach(var stores in StoreStocks)
            {
                foreach(var models in stores.Value)
                {
                    if(models.Stock.contract || models.Stock.unlocked)
                    {
                        if (!output.ContainsKey(stores.Key))
                            output.Add(stores.Key, new List<string>());

                        output[stores.Key].Add(models.ModelID);
                    }
                }
            }

            return output;
        }

        public void PrintStocks(bool OnlyStock = false)
        {
            foreach(var stores in StoreStocks)
            {
                if(storeInfo != null)
                {
                    var storeData = storeInfo.FindStore(stores.Key);
                    if (storeData != null)
                    {
                        Console.WriteLine(storeData.storeName);
                    }
                    else Console.WriteLine(stores.Key);
                }
                else Console.WriteLine(stores.Key);

                foreach(var models in stores.Value)
                {
                    if (OnlyStock)
                    {
                        if(models.Stock.contract || models.Stock.unlocked)
                        {
                            Console.WriteLine("\t{0} cont:{1} unlock:{2}", models.ModelID, models.Stock.contract.ToString(), models.Stock.unlocked.ToString());
                        }
                    }else
                        Console.WriteLine("\t{0} cont:{1} unlock:{2}", models.ModelID, models.Stock.contract.ToString(), models.Stock.unlocked.ToString());

                }
                Console.WriteLine();
            }
        }
    }
}
