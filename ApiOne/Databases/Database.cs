using ApiOne.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ApiOne.Databases
{
    public partial class Database
    {
        public Database(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;

        }

        private static Database dbconec;
        private readonly SqlConnection conn;
        string connectionString = "Data Source=DESKTOP-79B5CPA;Initial Catalog=notDB;Integrated Security=True";
        private readonly IHubContext<ChatHub> _myHub;

        private Database()
        {
            conn = new SqlConnection(connectionString);
        }
        //grabs instance of singleton pattern

        public static Database GetInstance()
        {
            if (dbconec == null) //check if  an instance has been created else  can create a new instance
            {
                dbconec = new Database();
            }
            return dbconec;
        }

        


    }
}
