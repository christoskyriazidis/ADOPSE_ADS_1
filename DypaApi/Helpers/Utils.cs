using DypaApi.Interfaces;
using DypaApi.Models.Weather;
using DypaApi.Models.Weather.SingleCall;
using DypaApi.Models.Xorafi;
using DypaApi.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DypaApi.Helpers
{
    public class Utils
    {
        private readonly IXorafi _xorafiRepo = new XorafiRepository();
        private readonly ISensor _sensorRepo = new SensorRepository();

        public  async Task RefreshHourlySensorLogs(Xorafi xorafi, ISensor _sensorRepo)
        {
            HttpClient httpClient = new HttpClient();
            string uri = $"https://api.openweathermap.org/data/2.5/onecall?lat={xorafi.Latitude}&lon={xorafi.Longitude}&exclude=hourly,daily,minutely&appid=2512835ec38ca99351ed383b16107efe&units=metric";

            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            var test = await httpClient.GetStringAsync(uri);
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                try
                {
                    var stats = await JsonSerializer.DeserializeAsync<WeatherNow>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    _sensorRepo.HourlySensorLogs(stats,xorafi.Id);
                    if (stats.current.pressure > 1000)
                    {
                        _xorafiRepo.SetWatering(xorafi.Id, false);
                    }
                    else if (stats.current.humidity < 60)
                    {
                        _xorafiRepo.SetWatering(xorafi.Id, true);
                    }
                }
                catch (JsonException) // Invalid JSON
                {
                    Debug.WriteLine("Invalid JSON.");
                }
            }
            else
            {
                Debug.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }
        }

        public  async Task RefreshWeeklyForecast(Xorafi xorafi, ISensor _sensorRepo)
        {
            HttpClient httpClient = new HttpClient();
            var key = Startup.StaticConfig.GetValue<string>("ApiKeys:Openweathermap");
            string uri = $"https://api.openweathermap.org/data/2.5/onecall?units=metric&lat={xorafi.Latitude}&lon={xorafi.Longitude}&exclude=hourly,minutely&appid={key}";
            
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            var test = await httpClient.GetStringAsync(uri);
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                try
                {
                    var weeklyWeatherForecast = await JsonSerializer.DeserializeAsync<WeeklyWeatherModel>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                    weeklyWeatherForecast.daily.RemoveAt(7);
                    _sensorRepo.WeeklyForecast(weeklyWeatherForecast, xorafi.Id);
                    foreach(var i in weeklyWeatherForecast.daily)
                    {
                        if (i.pressure > 1000)
                        {
                            _xorafiRepo.SetWatering(xorafi.Id, true);
                            break;
                        }
                    }
                }
                catch (JsonException) // Invalid JSON
                {
                    Debug.WriteLine("Invalid JSON.");
                }
            }
            else
            {
                Debug.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }
        }
    }
}
