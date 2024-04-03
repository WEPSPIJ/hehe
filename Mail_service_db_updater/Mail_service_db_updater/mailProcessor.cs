using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using Mail_service_db_updater.old_redundant;
using Microsoft.Extensions.Configuration;

namespace Mail_service_db_updater
{
    public class mailProcessor : ImailProcessor
    {
        private readonly ILogger<Worker> _logger;

        public mailProcessor(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;

        }
        public void processMail()  {
            var mailComps = findUnsent();  //main logic of code looping in the worker below
            bool mailSucess = SendMail(mailComps);
            if (mailSucess == true)
            {
                SqlRequest.editStatusDB(mailComps[0], "1"); //updating DB to tell it that the mail has been sent
                _logger.LogInformation("mail sent to:" + mailComps[1]);
            }
            else
            {
                _logger.LogInformation("Mail failed to send. Not updating DB.");
            }
        }
        static string connectionString = "Data Source=DEV-SQL-01;" + "Initial Catalog=WorkExperience;" + "User id=work_experience_user;" + "Password=txWgFdPe6xGkpnbtRaq6;";
        public static List<string> findUnsent()  //method to find unsent emails
        {
            string queryString = "SELECT * FROM [WorkExperience].[dbo].[EmailQueue] WHERE [Sent] = 0";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); //connnects to server
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var list = new List<string>
                            {
                                reader.GetInt32(0).ToString(),
                                reader.GetString(1).ToString(),
                                reader.GetString(2).ToString(),
                                reader.GetString(3).ToString(),
                                reader.GetBoolean(4).ToString()
                            };
                            return list;
                        }
                        return new List<string> { "", "", "", "null", "null", "null" };
                    }
                }
            }
        }
        public static int editStatusDB(string id, string val)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE [WorkExperience].[dbo].[EmailQueue] SET Sent = @value WHERE Id =@id";
                command.Parameters.AddWithValue("@value", val);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                var amt_changed = command.ExecuteNonQuery();
                connection.Close();
                return amt_changed;
            }

        }

        public static bool SendMail(List<string> mailComps)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("557e1c43b15a8a", "d6ad817226e38d"),
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
