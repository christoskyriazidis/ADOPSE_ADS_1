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
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Ad]";
                    var ads = connn.Query<Ad>(sql).ToList();
                    return ads;
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public bool UpdateAd(int id,Ad ad)
        {
            string sql = "UPDATE [Ad] SET title=@title where id=@id";
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connn);
                    command.Parameters.Add("@id", SqlDbType.Int,30).Value=id;
                    command.Parameters.Add("@title", SqlDbType.VarChar,50).Value=ad.Title;
                    connn.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        public List<Category> GetCategories()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Category]";
                    var ads = connn.Query<Category>(sql).ToList();
                    return ads;

                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        public List<Condition> GetCondition()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Condition]";
                    var ads = connn.Query<Condition>(sql).ToList();
                    return ads;
                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public List<Manufacturer> GetManufacturer()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Manufacturer]";
                    var ads = connn.Query<Manufacturer>(sql).ToList();
                    return ads;
                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        public List<Ad> GetState()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Category]";
                    var ads = connn.Query<Ad>(sql).ToList();
                    return ads;
                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        public List<Ad> GetType()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Category]";
                    var ads = connn.Query<Ad>(sql).ToList();
                    return ads;
                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
        public List<Ad> GetWishList()
        {
            try
            {
                using (SqlConnection connn = new SqlConnection(connectionString))
                {
                    connn.Open();
                    string sql = "SELECT * from [Category]";
                    var ads = connn.Query<Ad>(sql).ToList();
                    return ads;
                }
                }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
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
