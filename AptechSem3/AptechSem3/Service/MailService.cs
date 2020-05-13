using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AptechSem3.Service
{
    public class MailService
    {
        public bool sendMail()
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("n17dcat055@student.ptithcm.edu.vn");
                    mail.To.Add("anhquanala@gmail.com");
                    mail.Subject = "Test send mail";
                    mail.Body = "<h3>Test message<h3>";
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 5877))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("n17dcat055@student.ptithcm.edu.vn", "anhquanala99");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}