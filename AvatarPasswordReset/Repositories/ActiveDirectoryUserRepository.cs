using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AvatarPasswordReset.Models;
using System.DirectoryServices;
using System.Configuration;

namespace AvatarPasswordReset.Repositories
{
    public class ActiveDirectoryUserRepository
    {
        public ActiveDirectoryUser GetUserById(string Id)
        {
            var username = ConfigurationManager.AppSettings["ADusername"].ToString();
            var password = ConfigurationManager.AppSettings["ADpassword"].ToString();
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://hmhc.local");
            
            DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
            try
            {
                var adUser = new ActiveDirectoryUser();
                directorySearcher.Filter = "(&(objectClass=user)(objectCategory=Person)(userPrincipalName=" + Id + "@hmhc.local))";
                directorySearcher.PropertiesToLoad.Add("mail");
                directorySearcher.PropertiesToLoad.Add("cn");
                directorySearcher.PropertiesToLoad.Add("userPrincipalName");
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult == null)
                {
                    adUser = null;
                }
                else
                {
                    adUser.Id = searchResult.Properties["userPrincipalName"][0].ToString().Split('@')[0];
                    adUser.Description = searchResult.Properties["cn"][0].ToString();
                    adUser.Email = searchResult.Properties["mail"][0].ToString();
                }
                directoryEntry.Close();
                directorySearcher.Dispose();
                return adUser;
            }
            catch (Exception)
            {
                directoryEntry.Close();
                directorySearcher.Dispose();
                return null;
            }
        }
    }
}