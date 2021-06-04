using DypaApi.Helpers;
using DypaApi.Hubs;
using DypaApi.Interfaces;
using DypaApi.Models.Weather;
using DypaApi.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DypaApi.CronJobs
{
    public class CronJob : ICronJob
    {
        private readonly ISensor _sensorRepo = new SensorRepository();
        private readonly IXorafi _xorafiRepo = new XorafiRepository();
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly Utils _utils;
        //private readonly Utils _utils = new Utils();

        public CronJob(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
            _utils = new Utils(_notificationHub);
        }

        public async Task FetchSensorAsync()
        {
            var xorafia = _xorafiRepo.GetXorafia();
            foreach (var i in xorafia)
            {
                await _utils.RefreshHourlySensorLogs(i, _sensorRepo);
            }
        }

    public async Task WeeklyForecastAsync() {
            var xorafia = _xorafiRepo.GetXorafia();
            foreach (var i in xorafia)
            {
                await _utils.RefreshWeeklyForecast(i, _sensorRepo);
                //await _notificationHub.Clients.All.SendAsync("WeeklyForecast");
            }
        }

    }
}
