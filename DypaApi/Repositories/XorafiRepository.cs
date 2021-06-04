using Dapper;
using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models.Notifications;
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
                string sql = "insert into category (title,imgUrl,OptimalSoilMoisture,LowestNormalSoilMoisture,UpperNormalSoilMoisture,weeklyRootWaterWinter,weeklyRootWaterSummer,ownerid) values " +
                    "(@title,@imgUrl,@OptimalSoilMoisture,@LowestNormalSoilMoisture,@UpperNormalSoilMoisture,@weeklyRootWaterWinter,@weeklyRootWaterSummer,@OwnerId)";
                var inserted = conn.Execute(sql, new { 
                    category.Title,
                    category.ImgUrl,
                    category.OptimalSoilMoisture,
                    category.LowestNormalSoilMoisture,
                    category.UpperNormalSoilMoisture,
                    category.WeeklyRootWaterWinter,
                    category.WeeklyRootWaterSummer,
                    category.OwnerId
                });
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
                string sql = "insert into subcategory (categoryid,title,ImageUrl) values (@categoryid,@title,@ImageUrl)";
                var inserted = conn.Execute(sql, new { 
                    subCategory.CategoryId,
                    subCategory.Title,
                    subCategory.ImageUrl
                });
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

                string sql = "exec add_xorafi_with_location @PresetId,@Latitude,@Longitude,@Title,@Owner,@Acres,@PlantRoots,@WaterSupply,@LocationTitle";
                var inserted = conn.Execute(sql, new { xorafi.PresetId,xorafi.Latitude,xorafi.Longitude,xorafi.Title,xorafi.Owner,xorafi.Acres,xorafi.PlantRoots,xorafi.WaterSupply,xorafi.LocationTitle });
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

        public IEnumerable<SubCategory> GetSubCategories(int CategoryId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM SubCategory where categoryid=@CategoryId";
                var subCategories = conn.Query<SubCategory>(sql,new { CategoryId}).ToList();
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
                string sql = "select x.*,l.title as locationTitle,l.latitude,l.longitude ,c.imgUrl,c.LowestNormalSoilMoisture,c.OptimalSoilMoisture,c.title as presetTitle,c.UpperNormalSoilMoisture,c.weeklyRootWaterSummer,c.weeklyRootWaterWinter,c.id as PresetId from xorafi x join location l on (x.id=l.xorafiid) join PresetPerXorafi p on(p.xorafiId=x.id) join Category c on (c.id=p.presetId)  WHERE x.id =@Xid";
                var xorafi = conn.Query<Xorafi>(sql,new { Xid}).FirstOrDefault();
                return xorafi;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<Xorafia> GetXorafiaByOwnerId(int OwnderId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select x.*,l.latitude,l.longitude,l.title as locationTitle," +
                    "(select top 1 temp from HourlySensorReport where xorafiId=x.id order by id desc ) as temp," +
                    "(select top 1 wind_speed from HourlySensorReport where xorafiId=x.id order by id desc ) as wind_Speed," +
                    "(select top 1 humidity from HourlySensorReport where xorafiId=x.id order by id desc ) as humidity," +
                    "(select top 1 pressure from HourlySensorReport where xorafiId=x.id order by id desc ) as pressure," +
                    "(select top 1 icon from HourlySensorReport where xorafiId=x.id order by id desc ) as icon," +
                    "c.imgUrl,c.title as presetTitle from xorafi x join location l on (l.xorafiId=x.id) join PresetPerXorafi p on (p.xorafiId=x.id) join Category c on (c.id=p.presetId)" +
                    " where x.owner=@OwnderId";
                var xorafia = conn.Query<Xorafia>(sql, new { OwnderId }).ToList();
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

        public List<XorafiWithPresetForSensor> GetXorafia()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select x.id,l.latitude,l.longitude,c.LowestNormalSoilMoisture,c.OptimalSoilMoisture,c.title,c.UpperNormalSoilMoisture,c.weeklyRootWaterSummer,c.weeklyRootWaterWinter,c.id as PresetId from xorafi x join location l on (x.id=l.xorafiid) join PresetPerXorafi p on(p.xorafiId=x.id) join Category c on (c.id=p.presetId)";
                var xorafia = conn.Query<XorafiWithPresetForSensor>(sql).ToList();
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

        public IEnumerable<Category> GetCategoriesByOwnerId(int OwnerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM Category where OwnerId=@OwnerId";
                var categories = conn.Query<Category>(sql,new { OwnerId}).ToList();
                return categories;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool AddPresetToXorafi(int XorafiId, int PresetId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "exec add_remove_preset @PresetId,@XorafiId";
                var inserted = conn.Execute(sql, new{ PresetId, XorafiId });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public IEnumerable<XorafiWithPreset> GetXorafiWithPreset(int XorafiId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select C.*,p.xorafiId from PresetPerXorafi p join Category c on(c.id=p.presetId) where p.xorafiId=@XorafiId";
                var xorafiWithPresets = conn.Query<XorafiWithPreset>(sql,new { XorafiId}).ToList();
                return xorafiWithPresets;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<XorafiNotification> GetXorafiNotifications(int OwnderId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select * from Notification where xorafiId in ((select id from Xorafi where owner =@OwnderId )) order by id desc";
                var xorafiNotifications = conn.Query<XorafiNotification>(sql, new { OwnderId }).ToList();
                return xorafiNotifications;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
    }
}
