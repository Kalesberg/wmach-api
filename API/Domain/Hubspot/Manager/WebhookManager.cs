using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace API.Manager
{
    public static class WebhookManager
    {
        public static string ComputeHash(string value)
        {
            var hash = SHA256.Create();

            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(value));
            return HexStringFromBytes(bytes);
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}