using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Weather
{
    public class DailyWeather
    {
        public long dt { get; set; }
        public long sunrise { get; set; }
        public TempModel temp { get; set; }

        public IEnumerable<WeatherDescription> weather { get; set; }
        public float  pressure { get; set; }
        public float  humidity{ get; set; }
        public decimal  wind_speed{ get; set; }
        public float  wind_deg{ get; set; }
    }
}
