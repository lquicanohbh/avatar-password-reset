using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using AvatarPasswordReset.Helpers;
using AvatarPasswordReset.Models;
using AvatarPasswordReset.UserManagement;
using AvatarPasswordReset.ViewModels;

namespace AvatarPasswordReset.Repositories
{
    public class AvatarUserRepository
    {
        public string SystemCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AvatarUserRepository()
        {
            this.SystemCode = ConfigurationManager.AppSettings["SystemCode"].ToString();
            this.Username = ConfigurationManager.AppSettings["Username"].ToString();
            this.Password = ConfigurationManager.AppSettings["Password"].ToString();
        }
        public AvatarUser GetUserById(string Id)
        {
            var avatarUser = new AvatarUser();
            var connectionString = ConfigurationManager.ConnectionStrings["AvatarDBPM"].ConnectionString;
            var commandText = "SELECT u.USERID as Id, " +
                                "u.user_description as Description " +
                                "FROM SYSTEM.Radplus_users u " +
                                "WHERE USERID=? ";
            using (var connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                using (var dbcommand = new OdbcCommand(commandText, connection))
                {
                    dbcommand.Parameters.Add(new OdbcParameter("USERID", Id));
                    using (var reader = dbcommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            avatarUser.Id = reader["Id"].ToString();
                            avatarUser.Description = reader["Description"].ToString();
                        }
                        else
                        {
                            avatarUser = null;
                        }
                    }
                }
                connection.Close();
            }
            return avatarUser;
        }
        public IEnumerable<AvatarUser> SearchUser(string Name)
        {
            var avatarUsers = new List<AvatarUser>();
            var connectionString = ConfigurationManager.ConnectionStrings["AvatarDBPM"].ConnectionString;
            var commandText = "SELECT TOP(10) u.USERID as Id, " +
                                "u.user_description as Description " +
                                "FROM SYSTEM.Radplus_users u " +
                                "WHERE u.user_description like ? " +
                                "AND u.active = 'Yes' " +
                                "ORDER BY u.user_description ";
            using (var connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                using (var dbcommand = new OdbcCommand(commandText, connection))
                {
                    dbcommand.Parameters.Add(new OdbcParameter("user_description", "%" + Name.ToUpper() + "%"));
                    using (var reader = dbcommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var avatarUser = new AvatarUser();
                            avatarUser.Id = reader["Id"].ToString();
                            avatarUser.Description = reader["Description"].ToString().Replace("\"", "");
                            avatarUsers.Add(avatarUser);
                        }
                    }
                }
                connection.Close();
            }
            return avatarUsers;
        }
        public WebServiceResponse ResetPassword(string Id)
        {
            var webSvc = new WebServices();
            try
            {
                var response = webSvc.GeneratePassword(SystemCode, Username, Password, Id);

                return response;
            }
            catch (Exception ex)
            {
                webSvc.Abort();
                return null;
            }
        }
        public void LogReset(HttpRequestBase request, ResetPassword resetPassword)
        {
            try
            {
                StreamWriter sw = File.AppendText(ConfigurationManager.AppSettings["FilePath"].ToString() +
                "SelfPasswordResetLog_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                sw.WriteLine(DateTime.Now.ToString() + "***" + request.UserHostAddress + "***" +
                    resetPassword.AvatarUserId + "***" + resetPassword.AvatarUserDescription +
                    resetPassword.ADUsername + "***" + resetPassword.ADEmail);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}