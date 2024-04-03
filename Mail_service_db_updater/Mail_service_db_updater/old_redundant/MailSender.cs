using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace Mail_service_db_updater.old_redundant
{

    internal class MailSender
    {
        public static bool SendMail(List<string> mailComps)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("36a41c1e921472", "bb70ad1073f12a"),
                EnableSsl = true
            };
            try
            {
                client.Send("mailtrap@demomailtrap.com", "work.experience@psp-it.co.uk", mailComps[2], mailComps[3]);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
