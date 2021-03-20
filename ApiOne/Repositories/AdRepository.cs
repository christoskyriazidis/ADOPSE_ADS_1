using ApiOne.Interfaces;
using ApiOne.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class AdRepository : IAdRepository
    {
        readonly string connectionString = "Data Source=DESKTOP-79B5CPA;Initial Catalog=adDB;Integrated Security=True";
        private IConfiguration _config;

        public AdRepository(IConfiguration config)
        {
            _config = config;
        }
        public AdRepository()
        {

        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("SqlServer"));
            }
        }



        public Ad GetAd(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * from [Ad] where id=@Id";
                    var ad = conn.Query<Ad>(sql, new { Id = id }).FirstOrDefault();
                    return ad;
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
        

        public IEnumerable<Ad> GetAds()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * from [Ad]";
                var ads = conn.Query<Ad>(sql).ToList();
                return ads;
            }
        }

        public bool DeleteAd(Ad ad)
        {
            throw new NotImplementedException();
        }

        public bool InsertAd(Ad ad)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "  INSERT INTO [Ad] (title,Description,Date,State,Img,Type,Category,Condition,Customer,Manufacturer)" +
                                "VALUES (@Title,@Description,@Date,@State,@Img,@Type,@Category,@Condition,@Customer,@Manufacturer)";
                    var result = conn.Execute(sql, new {
                    ad.Title,ad.Description,
                    Date = DateTime.Now.ToString(),ad.State,
                    ad.Img,ad.Type,
                    ad.Category,ad.Condition,
                    ad.Customer,ad.Manufacturer
                    });
                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public Ad UpdateAd(Ad ad)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = " UPDATE [Ad] SET title=@Title,Description=@Description,Date=@Date,State=@State,Img=@Img,Type=@Type,Category=@Category,Condition=@Condition," +
                                    "Customer=@Customer,Manufacturer=@Manufacturer where id=@Id";
                   var result = conn.Execute(sql, new
                    {
                       ad.Id,
                       ad.Title,
                       ad.Description,
                       Date = DateTime.Now.ToString(),
                       ad.State,
                       ad.Img,
                       ad.Type,
                       ad.Category,
                       ad.Condition,
                       ad.Customer,
                       ad.Manufacturer
                   });
                    return ad;
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            };
        }
    }
}
