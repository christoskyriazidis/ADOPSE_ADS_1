using identityServerNew.Controllers;
using identityServerNew.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Helpers
{
    public class SqlServerHelpers
    {
        public static bool InsertIntoDb(string username,string email,string name,string lastname,string address, string subid,string mobilePhone, LocationModel locationModel)
        {
            using (SqlConnection connection = new SqlConnection(Startup._config.GetConnectionString("AdDb")))
            {
                String query = "INSERT INTO [customer] (username,name,email,lastname,address,subid,MobilePhone,coordinates) VALUES (@username,@name,@email,@lastname,@address,@subid,@MobilePhone,@coordinates)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@subid", subid);
                    command.Parameters.AddWithValue("@MobilePhone", mobilePhone);
                    command.Parameters.AddWithValue("@coordinates", $"{locationModel.Latitude},{locationModel.Longitude}");

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    // Check Error
                    if (result < 0)
                        return false;

                    return true;
                     
                }
            }
        } 
        public static void LoginLogs(string SubId,string IpAddress)
        {
            using (SqlConnection connection = new SqlConnection(Startup._config.GetConnectionString("AdDb")))
            {
                String query = "EXEC login_logs @SuBId,@IpAddr";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SuBId", SubId);
                    command.Parameters.AddWithValue("@IpAddr", IpAddress);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                }
            }
        }

        public static bool InsertIntoDypaDb(string username, string email, string name, string lastname, string address, string subid, string mobilePhone, LocationModel locationModel)
        {
            using (SqlConnection connection = new SqlConnection(Startup._config.GetConnectionString("SmartXorafi")))
            {
                String query = "INSERT INTO [Users] (username,email,address,coords,subid,MobilePhone,name,LastName,role) VALUES " +
                    "(@username,@email,@address,@coords,@subid,@mobilephone,@name,@lastname,@role)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@lastname", lastname);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@subid", subid);
                    command.Parameters.AddWithValue("@mobilephone", mobilePhone);
                    command.Parameters.AddWithValue("@role", 1);
                    command.Parameters.AddWithValue("@coords", $"{locationModel.Latitude},{locationModel.Longitude}");

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
