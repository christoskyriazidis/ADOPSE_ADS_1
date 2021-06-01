using Dapper;
using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models.PostRequest;
using DypaApi.Models.Worker;
using DypaApi.Models.Xorafi;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Repositories
{
    public class XorafiRepository : IXorafi
    {
        public bool AddCategory(Category category)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "insert into category (title) values (@title)";
                var inserted = conn.Execute(sql, new { category.Title });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }
        public bool AddSubCategory(SubCategory subCategory)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "insert into subcategory (categoryid,title) values (@categoryid,@title)";
                var inserted = conn.Execute(sql, new { subCategory.CategoryId,subCategory.Title});
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool AddSensorToXorafi(SensorToXorafi sensorToXorafi)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC Add_Sensor_To_Xorafi @SensorId,@XorafiId";
                var inserted = conn.Execute(sql, new { sensorToXorafi.SensorId,sensorToXorafi.XorafiId });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool AddXorafi(Xorafi xorafi)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();

                string sql = "exec add_xorafi_with_location @Latitude,@Longitude,@Title,@Owner,@Acres,@PlantRoots,@WaterSupply,@LocationTitle";
                var inserted = conn.Execute(sql, new { xorafi.Latitude,xorafi.Longitude,xorafi.Title,xorafi.Owner,xorafi.Acres,xorafi.PlantRoots,xorafi.WaterSupply,xorafi.LocationTitle });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool EditXorafi(Xorafi xorafi)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM Category";
                var categories = conn.Query<Category>(sql).ToList();
                return categories;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<SubCategory> GetSubCategories()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM SubCategory";
                var subCategories = conn.Query<SubCategory>(sql).ToList();
                return subCategories;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public Xorafi GetXorafi(int Xid)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT TOP(1) * FROM xorafi WHERE id =@Xid";
                var xorafi = conn.Query<Xorafi>(sql,new { Xid}).FirstOrDefault();
                return xorafi;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<Xorafi> GetXorafiaByOwnerId(int OwnderId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select x.*,l.latitude,l.longitude,l.title from xorafi x join location l on (l.xorafiId=x.id) WHERE owner =@OwnderId";
                var xorafia = conn.Query<Xorafi>(sql, new { OwnderId }).ToList();
                return xorafia;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool RemoveXorafi(int xid)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "DELETE FROM Xorafi WHERE id=@xid";
                var inserted = conn.Execute(sql, new { xid });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public List<Xorafi> GetXorafia()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select x.*,l.title,l.latitude,l.longitude from xorafi x join location l on (x.id=l.xorafiid)";
                var xorafia = conn.Query<Xorafi>(sql).ToList();
                return xorafia;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool SetWatering(int XorafiId, bool state)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "update Xorafi set watering=@state where id=@XorafiId";
                var updated = conn.Execute(sql, new { state,XorafiId });
                return updated > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool WaterXorafi(int XorafiId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC add_water_date @XorafiId";
                var inserted = conn.Execute(sql, new { XorafiId });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }
    }
}
