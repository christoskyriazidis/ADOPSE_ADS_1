using identityServerNew.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Helpers
{
    public class SqlServerHelpers
    {
        public static bool InsertIntoDb(string username,string email,string name,string lastname,string address, string subid,string mobilePhone)
        {
            using (SqlConnection connection = new SqlConnection(Startup._config.GetConnectionString("AdDb")))
            {
                String query = "INSERT INTO [customer] (username,name,email,lastname,address,subid,MobilePhone) VALUES (@username,@name,@email,@lastname,@address,@subid,@MobilePhone)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@subid", subid);
                    command.Parameters.AddWithValue("@MobilePhone", mobilePhone);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    // Check Error
                    if (result < 0)
                        return false;

                    return true;
                     
                }
            }
        }
    }
}
