using DypaApi.Helpers;
using DypaApi.Interfaces;
using DypaApi.Models;
using DypaApi.Models.Weather;
using DypaApi.Models.Weather.SingleCall;
using DypaApi.Models.Xorafi;
using DypaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    //[Authorize]
    public class SensorController : Controller
    {
        private readonly ISensor _sensorRepo = new SensorRepository();
        private readonly IXorafi _xorafiRepo = new XorafiRepository();

        [HttpPost]
        [Route("/sensor")]
        public IActionResult CreateSensor(string Title)
        {
            if (string.IsNullOrEmpty(Title))
            {
                return BadRequest(new { response = "Title cannot cannot be null or empty"});
            }
            if (_sensorRepo.AddSensor(Title))
            {
                return Json(new { response = $"{Title} added!"});
            }
            return BadRequest(new { response = "Something went wrong"});
        }
        
        [HttpGet]
        [Route("/sensor")]
        public IActionResult GetSensors()
        {
            var sensors = _sensorRepo.GetSensors();
            if (sensors.Any())
            {
                return Json(sensors);
            }
            return Json(new { response="There no sensors left"});
        }

        [HttpPut]
        [Route("/sensor")]
        public IActionResult UpdateSensor([FromBody] Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }   
            if (_sensorRepo.UpdateSensor(sensor))
            {
                return Json(new { response = $"sensor {sensor.Id} {sensor.Name} updated successfully" });
            }
            return Json(new { response="There no sensors left"});
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Testt(string Title)
        {
            return Json(new { response = "working"});
        }

        [HttpGet]
        [Route("/weekly")]
        public async Task<IActionResult> GetWorksttAsync()
        {
            HttpClient httpClient = new HttpClient();
            string uri = "https://api.openweathermap.org/data/2.5/onecall?units=metric&lat=40.629269&lon=22.947412&exclude=hourly,minutely&appid=2512835ec38ca99351ed383b16107efe";

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
                    _sensorRepo.WeeklyForecast(weeklyWeatherForecast,6);
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
            return Ok();
        }

        [HttpGet]
        [Route("/daily")]
        public async Task<IActionResult> GetWorksttAsync123(Xorafi xorafi)
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
            return Ok();
        }


        [HttpGet]
        [Route("/xorafi/hourly/{XorafiId}")]
        public IActionResult GetHourlyXorafiReport(int XorafiId)
        {
            var hourlyXorafiReport = _sensorRepo.GetHourlySensorXorafiReport(XorafiId);
            if (hourlyXorafiReport != null)
            {
                return Json(hourlyXorafiReport);
            }
            return BadRequest(new { response="Den uparxei to xorafi or error?"});
        }
        
        [HttpGet]
        [Route("/xorafi/weekly")]
        public IActionResult GetWeeklyXorafiReport(int XorafiId,int PageNumber)
        {
            var hourlyXorafiReport = _sensorRepo.GetWeeklyForecastXorafiReports(XorafiId,PageNumber);
            if (hourlyXorafiReport != null)
            {
                return Json(hourlyXorafiReport);
            }
            return BadRequest(new { response="Den uparxei to xorafi or error?"});
        }

    }
}
