using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace API.Utilities
{
    public static class IPAddress
    {
        public static string GetIPAddress()
        {
            string hostname = Environment.MachineName;
            //HttpContext httpContext = HttpContext.Current;
            //var ipAdd = httpContext.Request.ServerVariables["HTTP_x_FORWARDED_FOR"];

            //if (string.IsNullOrEmpty(ipAdd))
            //{
            //    string[] addresses = ipAdd.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}

            //return httpContext.Request.ServerVariables["REMOTE_ADDR"];

            IPHostEntry iPHostEntry = Dns.GetHostEntry(hostname);
            foreach (var ip in iPHostEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return Convert.ToString(ip);
                }
            }

            return null;
        }
    }
}