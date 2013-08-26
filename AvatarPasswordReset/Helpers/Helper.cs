using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AvatarPasswordReset.Helpers
{
    public static class Helper
    {
        public static void EmailPassword(string RecipientEmail, string Password)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                mailMessage.From = new MailAddress("AvatarSupport@hendersonbehavioralhealth.org");
                mailMessage.To.Clear();
                mailMessage.To.Add(RecipientEmail);
                mailMessage.Subject = "New Avatar Password";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<font size='4'><p>Your temporary Avatar password is <b>" + Password + "</b></p>" +
                                "<p>Please make sure to enter your password exactly as it appears here (you can copy and paste this into Avatar).</p>" +
                                "<p>Remember Avatar passwords are case sensitive and the system will prompt you to change your temporary password after the first time you log in.</p></font>" +
                                "<font size='2'><p>Notice: This e-mail was auto-generated. This is a no-reply e-mail address.<br/>" +
                                "If you are still having problems with your avatar password please submit a spiceworks ticket.</p>";
                smtpClient.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }
    }
}