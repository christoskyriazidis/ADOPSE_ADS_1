using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class XorafiWithPreset
    {
        public int Id { get; set; }
        public int XorafiId { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public int OptimalSoilMoisture { get; set; }
        public int LowestNormalSoilMoisture { get; set; }
        public int UpperNormalSoilMoisture { get; set; }
        public int weeklyRootWaterWinter { get; set; }
        public int weeklyRootWaterSummer { get; set; }

    }
}
