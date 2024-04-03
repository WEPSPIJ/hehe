using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Mail_service_db_updater.old_redundant
{

    internal class SqlRequest
    {
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
    }
}
