using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using API.Data;
using API.Utilities.Password;
using System.Configuration;
using API.Models;
using System.Web.Http.Controllers;

namespace API.Utilities.Auth
{
    public class CustomerPortalAuth
    {
        public Dictionary<string, bool> IsAuthenticate(string Email, string Password)
        {

            Dictionary<string, bool> Auth = new Dictionary<string, bool>();

            string PwdStoredHashed = null;

            bool Accesstoportal = false;

            var PwdObj = new API.Utilities.Password.Password();

            //Search username in CustomerPortalUser table

            var db = DAL.GetInstance();

            var CustomerPortalUser = db.GetCustomerPortalUserByEmail(Email);

            if (CustomerPortalUser.Count() > 0)
            {
                //Check if password is valide               

                // Get password crypted and stored in CustomerPortalUser

                foreach (var col in CustomerPortalUser)
                {
                    PwdStoredHashed = col.MyAccountPassword;
                    Accesstoportal = col.AccesstoPortal;
                }

                if (PwdObj.ValidatePassword(Password, PwdStoredHashed))
                {
                    //User account has access to customer portal
                    if (Accesstoportal)
                    {
                        Auth.Add("AcoountValide", true);
                        Auth.Add("AccestoPortal", true);
                        return Auth;
                    }
                    else
                    {
                        Auth.Add("AcoountValide", true);
                        Auth.Add("AccestoPortal", false);
                        return Auth;
                    }
                }
                else
                {
                    Auth.Add("AcoountValide", false);
                    Auth.Add("AccestoPortal", false);
                    return Auth;
                }

            }

            else
            {
                Auth.Add("AcoountValide", false);
                Auth.Add("AccestoPortal", false);
                return Auth;
            }


        }



        //CREATE TOKEN. TOKEN IS VALID FOR 24 HOURS. IF USER IS IN M2.ExtendedUserLogin THEN OVERRIDE DEFAULT VALUE.
        public string GetToken(IEnumerable<MemberShipUser> user, int loggedInDuration = 0)
        {
            string FirstName = null;
            string LastName = null;
            string Role = null;
            int CustomerId = 0;
            var secondsIn24Hours = 31536000;
            //var contactID = DAL.GetInstance().GetContactIDByUsername(user.UserName);
            var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var duration = loggedInDuration == 0 ? Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + secondsIn24Hours) : Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds + (secondsIn24Hours * loggedInDuration));

            foreach (var col in user)
            {
                FirstName = col.FirstName;
                LastName = col.LastName;
                Role = col.Role;
                CustomerId = col.CustomerID;
            }

            var payload = new Dictionary<string, object>()
            {
                { "exp", duration},               
                { "FirstName", FirstName },
                { "LastName", LastName },
                { "IsExt", "1" },
                { "Role", Role},
                { "CustomerId", CustomerId},
                { "StayLoggin" , true}
            };

            return JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
        }



        //Correct the link
        //<Parameter name="LinkFormatedBase64String">

        public string FormatLink(string Link)
        {
            string LinkFormated = string.Empty;

            if (Link.Contains("+") && Link.Contains("/"))
            {
                LinkFormated = Link.Replace("+", "-");
                LinkFormated = LinkFormated.Replace("/", "_");
            }

            if (Link.Contains("+") && !Link.Contains("/"))
            {
                LinkFormated = Link.Replace("+", "-");
            }

            if (!Link.Contains("+") && Link.Contains("/"))
            {
                LinkFormated = Link.Replace("/", "_");
            }

            if (!Link.Contains("+") && !Link.Contains("/"))
            {
                LinkFormated = Link;
            }


            return LinkFormated;



        }
        //Generate Guid with parameter
        //<Parameter name="DateTimeNow">

        public string GenerateGuidByTimeStamp(DateTime TimeStamp)
        {

            byte[] buf = Encoding.UTF8.GetBytes(TimeStamp.ToString());
            byte[] guid = new byte[buf.Length];
            Array.Copy(buf, guid, buf.Length);

            //Convert string to Hexadecimal

            var HexString = BitConverter.ToString(guid);

            //remove "-" caractere

            HexString = HexString.Replace("-", "");


            return HexString;



        }

        public int GetCustomerIDFromToken(HttpActionContext ctx)
        { 
            
            string token =  ctx.Request.Headers.Authorization.Parameter;
            var secretKey = ConfigurationManager.AppSettings["CustomerPortalAuth"];
            string jsonPayload= JWT.JsonWebToken.Decode(token, secretKey);
            //Convert jsonPayload to JsonObject
             var JsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonPayload);
             if (JsonObject["CustomerId"].ToString().Length > 0)
                 return JsonObject["CustomerId"];
            else
                return 0;
        }

    }
}
