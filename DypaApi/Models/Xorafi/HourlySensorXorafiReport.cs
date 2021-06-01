using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class HourlySensorXorafiReport
    {
        public int Id { get; set; }
        public int XorafiId { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public string Timestamp { get; set; }
        public string Icon { get; set; }
        public decimal Wind_speed { get; set; }
        public decimal Wind_deg { get; set; }
        public decimal Temp { get; set; }
        public decimal Visibility { get; set; }
        public int ConditionId { get; set; }
    }
}
