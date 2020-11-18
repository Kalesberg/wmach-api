using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using API.Utilities.Auth;
using API.Models;
using System.Configuration;

namespace API.Utilities
{
    public class ActiveDirectory
    {
        /**
         * With two serperate root directories, make sure you connect to the correct one.
         * Here is the path to "LDAP://CORP.WWMACH.COM" and "CN=WWM-SG-DC01,OU=Domain Controllers,DC=corp,DC=wwmach,DC=com"
         **/

        static string _domain = "wwmach";
        static string _container = "DC=corp,DC=wwmach,DC=com";

        public static string GetActiveDirectoryEmail(string jwt)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["LDAP"], ConfigurationManager.AppSettings["LDAPEmail"], ConfigurationManager.AppSettings["LDAPPassword"]);
                DirectorySearcher search = new DirectorySearcher(entry);

                //GET CURRENT USER FROM JWT
                var userName = Authentication.GetUserName(jwt);

                //SEARCH FILTER
                search.Filter = "(&(objectClass=user)(anr=" + userName + "))";

                //LOAD EMAIL PROP
                search.PropertiesToLoad.Add("mail");

                //EXECUTE SEARCH
                SearchResult result = search.FindOne();
                return result.Properties["mail"][0].ToString();
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static Contact GetActiveDirectoryContactInfo(string userName)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["LDAP"], ConfigurationManager.AppSettings["LDAPEmail"], ConfigurationManager.AppSettings["LDAPPassword"]);
                DirectorySearcher search = new DirectorySearcher(entry);

                //SEARCH FILTER
                search.Filter = "(&(objectClass=user)(anr=" + userName + "))";

                //LOAD EMAIL PROP
                search.PropertiesToLoad.Add("memberof");
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("GivenName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("ADsPath");

                //EXECUTE SEARCH
                SearchResult result = search.FindOne();
                var contact = new Contact();

                foreach (var member in result.Properties["memberof"])
                {
                    var from = member.ToString().IndexOf("CN=") + "CN=".Length;
                    var to = member.ToString().IndexOf(",");
                    contact.Groups.Add(member.ToString().Substring(from, to - from));
                }
               
                contact.UserName = result.Properties["samaccountname"][0].ToString();
                contact.FirstName = result.Properties["GivenName"][0].ToString();
                contact.LastName = result.Properties["sn"][0].ToString();
                var ex = result.Properties["ADsPath"][0].ToString();
                return contact;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static UserPrincipal GetPrincipalByEmail(string emailAddress)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, _container);
                UserPrincipal user = new UserPrincipal(context);
                user.EmailAddress = emailAddress;

                PrincipalSearcher ps = new PrincipalSearcher(user);

                return (UserPrincipal)ps.FindOne();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<UserPrincipal> GetMembersInGroup(string groupName)
        {
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, _container);
                GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);

                return group.Members.Select(principal => 
                    principal as UserPrincipal).ToList<UserPrincipal>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}