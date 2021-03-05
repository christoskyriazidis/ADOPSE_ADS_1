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
    public class Database
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

        public void getprodd() {
            string connectionString = "Data Source=DESKTOP-79B5CPA;Initial Catalog=notDB;Integrated Security=True";
            using (SqlTableDependency<Client> dep =
                 new SqlTableDependency<Client>(connectionString))
            {
                dep.OnChanged += Dep_OnChanged;
                dep.OnError += SqlTableDependency_OnError;
                dep.Start();
                Debug.WriteLine("press enter gia na pareis poulo");
                Console.ReadLine();

            }

        }

        private void SqlTableDependency_OnError(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine("error");
        }

        private async void Dep_OnChanged(object sender,
          RecordChangedEventArgs<Client> e)
        {

            switch (e.ChangeType)
            {
                case TableDependency.SqlClient.Base.Enums.ChangeType.None:
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Delete:
                    Debug.WriteLine(e.Entity.title + " deleted");
                    await _myHub.Clients.All.SendAsync("ReceiveMessage", "db", $"deleted {e.Entity.title}");
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Insert:
                    await _myHub.Clients.All.SendAsync("ReceiveMessage", "db", $"inserted {e.Entity.title}");
                    break;
                case TableDependency.SqlClient.Base.Enums.ChangeType.Update:
                    Debug.WriteLine(e.Entity.title + " updated");
                    await _myHub.Clients.All.SendAsync("ReceiveMessage", "db", $"updated:{e.Entity.title}");
                    break;
                default:
                    break;
            }
        }
    }
}
