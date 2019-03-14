using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LibraryProject.Common
{
    public class Email
    {
       
        public static bool Send(string _email, string _name, string _message)
        {
            StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/ContactMail.txt"));
            string Email = file.ReadLine();
            string Password = file.ReadLine();





            MailMessage mailmessage = new MailMessage(_email, Email);
            mailmessage.Subject = "Basic Admin " + _name;
            mailmessage.Body = _message;
            mailmessage.IsBodyHtml = true;


            SmtpClient smtpclient = new SmtpClient("smtp.gmail.com", 587);
            smtpclient.EnableSsl = true;

            smtpclient.Credentials = new System.Net.NetworkCredential(Email, Password);



            try
            {
                smtpclient.Send(mailmessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}