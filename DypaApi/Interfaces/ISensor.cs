using DypaApi.Models;
using DypaApi.Models.Weather;
using DypaApi.Models.Weather.SingleCall;
using DypaApi.Models.Xorafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Interfaces
{
    public interface ISensor
    {
        bool AddSensor(string Name);

        bool UpdateSensor(Sensorr sensor);
        bool WeeklyForecast(WeeklyWeatherModel weeklyWeatherModel,int XorafiId);
        bool HourlySensorLogs(WeatherNow weatherNow, int XorafiId);
        IEnumerable<Sensorr> GetSensors();
        void Test(int xorafiId,int PageNumber);
        IEnumerable<WeeklyForecastXorafiReport> GetWeeklyForecastXorafiReports(int XorafiId,int PageNumber);
        HourlySensorXorafiReport GetHourlySensorXorafiReport(int XorafiId);


    }
}
