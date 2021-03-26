using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Helpers
{
    public class ConnectionManager
    {
        
        public static SqlConnection GetSqlConnection()
        {
            string connString = Startup.StaticConfig.GetConnectionString("Azure");
            var connection = new SqlConnection(connString);
            connection.Open();
            return connection;
        }
    }
}
