using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Web;
using API.Models;
using System.Configuration;
namespace API.Data
{
    public partial class DAL
    {
       
        public IEnumerable<MemberShipUser> GetCustomerPortalUserByGuid(string guid)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerPortalUserSelectByGuid"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@TimeStampLink", guid);           
            return getRecords<MemberShipUser>(cmdText);
        }


        public bool ResetPassword(string Email, string pwd, DateTime DatePasswordChanged,  string TimeStampEncrypted, string TimeStampLink) 
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerPortalUserResetPassword"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;           
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", pwd);
            cmd.Parameters.AddWithValue("@DatePasswordChanged", DatePasswordChanged);
            cmd.Parameters.AddWithValue("@ResetPwd", true);
            cmd.Parameters.AddWithValue("@TimeStampEncrypted", TimeStampEncrypted);
            cmd.Parameters.AddWithValue("@TimeStampLink", TimeStampLink);
           
            return  UpdateRecord(cmdText);

            
        }

        public IEnumerable<MemberShipUser>GetCustomerPortalUserByEmail(string Email)
        {
            string cmdText = ConfigurationManager.AppSettings["CustomerPortalUserSelectByEmail"];
            if (String.IsNullOrWhiteSpace(cmdText)) return null;
            cmd.Parameters.AddWithValue("@Email", Email);
            return getRecords<MemberShipUser>(cmdText);
        }


        public bool ResetPwdRequestCustomerPortaluser(string Email,  string TimeStampEncrypted, string TimeStampLink)
        {
            //Reset password request
            //Update CustomerPortalUser with  guids value

            string cmdText = ConfigurationManager.AppSettings["CustomerPortalUserResetPasswordRequest"];
            if (String.IsNullOrWhiteSpace(cmdText)) return false;
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@TimeStampEncrypted", TimeStampEncrypted);
            cmd.Parameters.AddWithValue("@TimeStampLink", TimeStampLink);
            
            return UpdateRecord(cmdText);

        }
        
    }
}