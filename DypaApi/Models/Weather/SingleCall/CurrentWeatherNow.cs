using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Weather.SingleCall
{
    public class CurrentWeatherNow
    {
        public long dt { get; set; }
        public decimal temp { get; set; }
        public long pressure { get; set; }
        public long humidity { get; set; }
        public long visibility { get; set; }
        public decimal wind_speed { get; set; }
        public decimal wind_deg { get; set; }
        public IEnumerable<WeatherDescription> weather { get; set; }

    }
}
