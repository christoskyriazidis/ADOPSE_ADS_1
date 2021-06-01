using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class WeeklyForecastXorafiReport
    {
        public int Id { get; set; }
        public int Timestamp { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int XorafiId { get; set; }
        public int ConditionId { get; set; }
        public decimal Wind_speed { get; set; }
        public decimal Wind_deg { get; set; }
        public decimal MinTemp { get; set; }
        public decimal MaxTemp { get; set; }
        public string Description { get; set; }
        public string Main { get; set; }
        public string Icon { get; set; }
    }
}
