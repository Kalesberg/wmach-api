using API.Data;
using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Text;

namespace API.Utilities.Auth
{
    public class Authentication
    {
        public static bool IsAuthenticated(string username, string password, out Contact contact)
        {
            try
            {
                //WILL THROW EXCEPTION IF AUTHENTICATION FAILS
                DirectoryEntry dirEntry = new DirectoryEntry("LDAP://" + "WWM.com", username, password);
                DirectorySearcher search = new DirectorySearcher(dirEntry);
                //SEARCH FILTER
                search.Filter = "(&(objectCategory=person)(objectClass=user)(anr=" + username + "))";
                //LOAD EMAIL PROP
                search.PropertiesToLoad.Add("memberof");
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("GivenName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("ADsPath");
                SearchResult result = search.FindOne();

                contact = new Contact();

                foreach (var member in result.Properties["memberof"])
                {
                    var from = member.ToString().IndexOf("CN=") + "CN=".Length;
                    var to = member.ToString().IndexOf(",");
                    contact.Groups.Add(member.ToString().Substring(from, to - from));
                }

                contact.UserName = result.Properties["samaccountname"][0].ToString();
                contact.FirstName = result.Properties["GivenName"][0].ToString();
                contact.LastName = result.Properties["sn"][0].ToString();

                object nativeObj = dirEntry.NativeObject;
                return true;
            }

            catch (Exception)
            {
                contact = new Contact();
                return false;
            }
        }

        //CREATE TOKEN. TOKEN IS VALID FOR 24 HOURS. IF USER IS IN M2.ExtendedUserLogin THEN OVERRIDE DEFAULT VALUE.
        public static string GetToken(Contact user, int loggedInDuration = 0)
        {
            var secondsIn24Hours = 86400;
            var contactID = DAL.GetInstance().GetContactIDByUsername(user.UserName);
            var secretKey = ConfigurationManager.AppSettings["AuthKey"];
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var duration = loggedInDuration == 0 ? Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + secondsIn24Hours) : Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + (secondsIn24Hours * loggedInDuration));
            var payload = new Dictionary<string, object>()
            //M1-1070
            //Add new parameter : "IsExt" for check if token is provided by internal or external user
            //By defaut IsExt = false because this API is used by Mobile app(all users over here are suppose to be internal user)
            {
                { "exp", duration},
                { "name", user.UserName},
                { "FirstName", user.FirstName },
                { "LastName", user.LastName },
                { "IsExt", "0" },
                { "contactID", contactID},
                { "Role", "Internal" },
                { "Security Group", user.Groups }
            };

            return JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
        }

        //CREATE TOKEN. TOKEN IS VALID FOR 24 HOURS. IF USER IS IN M2.ExtendedUserLogin THEN OVERRIDE DEFAULT VALUE.
        public static string GetToken(string userName, int loggedInDuration = 0)
        {
            var secondsIn24Hours = 86400;
            var contactID = DAL.GetInstance().GetContactIDByUsername(userName);
            var secretKey = ConfigurationManager.AppSettings["AuthKey"];
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var duration = loggedInDuration == 0 ? Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + secondsIn24Hours) : Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + (secondsIn24Hours * loggedInDuration));
            var payload = new Dictionary<string, object>()
            //M1-1070
            //Add new parameter : "IsExt" for check if token is provided by internal or external user
            //By defaut IsExt = false because this API is used by Mobile app(all users over here are suppose to be internal user)
            {
                { "exp", duration},
                { "name", userName},
                { "IsExt","0" },
                { "contactID", contactID}
            };

            return JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
        }

        public static string GetUserName(string token)
        {
            var startIndex = token.IndexOf(".") + 1;
            var endIndex = token.IndexOf(".", startIndex);
            var claimSubStr = token.Substring(startIndex, endIndex - startIndex);
            var subStrAppendLen = (claimSubStr.Length % 4);

            while (subStrAppendLen != 0)
            {
                claimSubStr = claimSubStr + "=";
                subStrAppendLen = (claimSubStr.Length % 4);
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(claimSubStr));
            var jObj = JObject.Parse(json);
            return jObj["name"].ToString();
        }

        public static Contact GetContactDetails(string token)
        {
            var startIndex = token.IndexOf(".") + 1;
            var endIndex = token.IndexOf(".", startIndex);
            var claimSubStr = token.Substring(startIndex, endIndex - startIndex);
            var subStrAppendLen = (claimSubStr.Length % 4);

            while (subStrAppendLen != 0)
            {
                claimSubStr = claimSubStr + "=";
                subStrAppendLen = (claimSubStr.Length % 4);
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(claimSubStr));
            var jObj = JObject.Parse(json);

            var contact = new Contact();
            contact.UserName = jObj["name"].ToString();
            contact.FirstName = jObj["FirstName"].ToString();
            contact.LastName = jObj["LastName"].ToString();

            return contact;
        }

        public static int GetUserExtendedLoginDuration(string userName)
        {
            return DAL.GetInstance().GetUserLoginDuration(userName);
        }
    }
}