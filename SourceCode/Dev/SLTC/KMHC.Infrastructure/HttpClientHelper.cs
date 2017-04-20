using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KMHC.Infrastructure
{
    public static class HttpClientHelper
    {
        private static string ltcAddress = "";
        public static string LtcAddress
        {
            get
            {
                if (string.IsNullOrEmpty(ltcAddress))
                {
                    ltcAddress = ConfigurationManager.AppSettings["ltcAddress"];
                }
                return ltcAddress;
            }
        }
        private static readonly object LtcLockObj = new object();

        private static HttpClient _ltcClient;

        public static HttpClient LtcHttpClient
        {
            get
            {
                if (_ltcClient == null)
                {
                    lock (LtcLockObj)
                    {
                        if (_ltcClient == null)
                        {
                            _ltcClient = new HttpClient() { BaseAddress=new Uri(LtcAddress) };
                        }
                    }
                }

                return _ltcClient;
            }
        }
    }
}
