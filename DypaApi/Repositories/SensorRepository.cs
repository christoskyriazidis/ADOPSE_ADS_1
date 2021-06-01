using Dapper;
using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models;
using DypaApi.Models.Weather;
using DypaApi.Models.Weather.SingleCall;
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
    public class SensorRepository : ISensor
    {
        public bool AddSensor(string Name)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "INSERT INTO SENSOR (Name) values (@Name)";
                var inserted = conn.Execute(sql,new{ Name});
                return inserted>0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public HourlySensorXorafiReport GetHourlySensorXorafiReport(int XorafiId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT TOP (1) * FROM HourlySensorReport WHERE xorafiid=@XorafiId ORDER BY ID DESC";
                var xorafiReport = conn.Query<HourlySensorXorafiReport>(sql,new { XorafiId }).FirstOrDefault();
                return xorafiReport;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<Sensor> GetSensors()
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "SELECT * FROM Sensor";
                var sensors = conn.Query<Sensor>(sql).ToList();
                return sensors;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<WeeklyForecastXorafiReport> GetWeeklyForecastXorafiReports(int XorafiId, int PageNumber)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "exec [dbo].[get_forecast_by_xorafi] @pageNumber, @XorafiId";
                var weeklyForecastXorafiReports = conn.Query<WeeklyForecastXorafiReport>(sql,new { pageNumber=PageNumber, XorafiId= XorafiId }).ToList();
                return weeklyForecastXorafiReports;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool HourlySensorLogs(WeatherNow weatherNow, int XorafiId)
        {
            try
            {
                int contId = 0;
                var iconName = "";
                foreach (var ii in weatherNow.current.weather)
                {
                    contId = ii.id;
                    iconName = ii.icon;
                }
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "exec sensor_logs_per_hour @XorafiId, @Pressure, @Humidity, @WindSpeed, @WindDeg, @Temp, @Visibility,@ConditionId,@Icon";
                var inserted = conn.Execute(sql, new { 
                    XorafiId,
                    Pressure=weatherNow.current.pressure,
                    Humidity = weatherNow.current.humidity,
                    WindSpeed=weatherNow.current.wind_speed,
                    WindDeg=weatherNow.current.wind_deg,
                    Temp=weatherNow.current.temp,
                    Visibility=weatherNow.current.visibility,
                    ConditionId=contId,
                    Icon=iconName
                });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool UpdateSensor(Sensor sensor)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "Update SENSOR SET Name=@Name where id=@Id";
                var inserted = conn.Execute(sql, new { sensor.Name,sensor.Id });
                return inserted > 0;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool WeeklyForecast(WeeklyWeatherModel weeklyWeatherModel,int XorafiId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                foreach (var i in weeklyWeatherModel.daily)
                {
                    var main = "";
                    var description = "";
                    int contId =0;
                    var iconId ="";
                    foreach(var ii in i.weather)
                    {
                        main = ii.main;
                        description = ii.description;
                        contId = ii.id;
                        iconId = ii.icon;
                    }
                    string sql = "INSERT INTO forecast (Timestamp,pressure,humidity,wind_speed,wind_deg,description,main,minTemp,maxTemp,xorafiId,conditionId,icon) VALUES " +
                        "(@Timestamp,@Pressure,@Humidity,@Wind_speed,@Wind_deg,@Description,@Main,@MinTemp,@MaxTemp,@xorafiId,@ConditionId,@Icon)";
                    conn.Execute(sql, new {
                        Timestamp=i.dt, 
                        Pressure=i.pressure,
                        Humidity=i.humidity,
                        Wind_speed=i.wind_speed,
                        Wind_deg=i.wind_deg,
                        Description= description,
                        Main= main,
                        MinTemp=i.temp.min,
                        MaxTemp=i.temp.max,
                        XorafiId,
                        ConditionId = contId,
                        Icon=iconId
                    });
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

    }
}
