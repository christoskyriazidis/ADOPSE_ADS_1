using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Weather
{
    public class WeeklyWeatherModel
    {
        public List<DailyWeather> daily { get; set; }
    }
}
