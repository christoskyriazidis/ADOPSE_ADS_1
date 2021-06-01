using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Weather
{
    public class SingleDailyWeather
    {
        public long dt { get; set; }
        public long temp { get; set; }
        public IEnumerable<WeatherDescription> weather { get; set; }
        public long pressure { get; set; }
        public float humidity { get; set; }
        public float wind_speed { get; set; }
        public float wind_deg { get; set; }
    }
}
