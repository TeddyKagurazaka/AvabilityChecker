using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Avability.Core
{
    public class StoreInfo
    {
        public class StoreTemp{
            public string city = "";
            public bool enabled = false;
            public string latitude = "";
            public string longitude = "";
            public string storeName = "";
            public string storeNumber = "";
        }

        Dictionary<string, StoreTemp> StoreData;
        Dictionary<string, object> ConfigInfo;

        public StoreInfo()
        {
            StoreData = new Dictionary<string, StoreTemp>();
            ConfigInfo = new Dictionary<string, object>();
        }

        public bool Update(string Channel = "A"){

            var data = Internals.Request(string.Format("https://reserve-prime.apple.com/CN/zh_CN/reserve/{0}/stores.json",Channel));
            if (string.IsNullOrEmpty(data)) return false;

            StoreData.Clear();
            ConfigInfo.Clear();

            var contents = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            ConfigInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>((contents["config"].ToString()));

            foreach (var stores in (JArray)contents["stores"])
            {
                StoreTemp storeInfo = JsonConvert.DeserializeObject<StoreTemp>(stores.ToString());
                StoreData.Add(storeInfo.storeNumber, storeInfo);
            }
            Console.WriteLine("StoreInfo Updated:" + StoreData.Count);

            return true;
        }

        public StoreTemp FindStore(string storeID)
        {
            if (StoreData.ContainsKey(storeID))
                return StoreData[storeID];
            return null;
        }
    }
}
