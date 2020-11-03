using System;
using System.Net;

namespace Avability.Core
{
    public static class Internals
    {
        //"https://reserve-prime.apple.com/CN/zh_CN/reserve/A/stores.json")
        public static string Request(string Url)
        {
            var data = "";

            try
            {
                var req = WebRequest.CreateHttp(Url);
                req.Timeout = 2000;
                var resp = req.GetResponse() as HttpWebResponse;

                using (var respReader = new System.IO.StreamReader(resp.GetResponseStream()))
                {
                    data = respReader.ReadToEnd();
                }

                return data;
            }
            catch
            {
                return "";
            }

        }
    }
}
