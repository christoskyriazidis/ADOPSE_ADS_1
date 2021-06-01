using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DypaApi.Helpers
{
    public class ConnectionManager
    {
        public static SqlConnection GetSqlConnection()
        {

            string connString = Startup.StaticConfig.GetConnectionString("DefaultConnection");

            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }
    }
}
