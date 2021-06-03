using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class XorafiWithPresetForSensor
    {
        public int Id { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int LowestNormalSoilMoisture { get; set; }
        public int OptimalSoilMoisture { get; set; }
        public int UpperNormalSoilMoisture { get; set; }
        public int WeeklyRootWaterSummer { get; set; }
        public int WeeklyRootWaterWinter { get; set; }
        public int PresetId { get; set; }
        public string Title { get; set; }

    }
}
