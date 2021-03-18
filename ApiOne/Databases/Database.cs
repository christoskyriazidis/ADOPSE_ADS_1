using ApiOne.Hubs;
using ApiOne.Models;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
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
        public Database(
           IConfiguration config)
        {
            _config = config;
        }

        private readonly IConfiguration _config;
        private static Database dbconec;
        readonly string connectionString = "Data Source=DESKTOP-79B5CPA;Initial Catalog=adDB;Integrated Security=True";
       
        private Database()
        {
        }

        public static Database GetInstance()
        {
            if (dbconec == null) //check if  an instance has been created else  can create a new instance
            {
                dbconec = new Database();
            }
            return dbconec;
        }

        public List<Ad> GetAds()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Ad]";
                var ads =  conn.Query<Ad>(sql).ToList();
                conn.Close();
                return ads;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }

        public bool UpdateAd(int id,Ad ad)
        {
            string sql = "UPDATE [Ad] SET title=@title where id=@id";
            try
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.Add("@id", SqlDbType.Int,30).Value=id;
                command.Parameters.Add("@title", SqlDbType.VarChar,50).Value=ad.Title;
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return false;
            }
        }

        public List<Category> GetCategories()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Category]";
                var ads =  conn.Query<Category>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }
        public List<Condition> GetCondition()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Condition]";
                var ads =  conn.Query<Condition>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }

        public List<Manufacturer> GetManufacturer()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Manufacturer]";
                var ads =  conn.Query<Manufacturer>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }
        public List<Ad> GetState()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Category]";
                var ads =  conn.Query<Ad>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }
        public List<Ad> GetType()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Category]";
                var ads =  conn.Query<Ad>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }
        public List<Ad> GetWishList()
        {
            try
            {
                conn.Open();
                string sql = "SELECT * from [Category]";
                var ads =  conn.Query<Ad>(sql).ToList();
                conn.Close();
                return ads;
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                conn.Close();
                return null;
            }
        }

        public List<WishListNotification> GetWishListNotification(int userId)
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = $"SELECT w.clicked,a.title,a.Img from [aWishListNotification] w join ad a on (w.adId=a.id) where w.customerId={userId}";
                    var ads = connn.Query<WishListNotification>(sql).ToList();
                    connn.Close();
                    return ads;
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        

    }
}
