using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class Xorafia
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Owner { get; set; }
        public int PlantRoots { get; set; }
        public int WaterSupply { get; set; }
        public bool Watering { get; set; }
        public  decimal Latitude { get; set; }
        public  decimal Longitude { get; set; }
        public  decimal Humidity { get; set; }
        public  string Icon { get; set; }
        public  decimal Pressure { get; set; }
        public  decimal Temp { get; set; }
        public  decimal Visibility { get; set; }
        public  decimal Wind_deg { get; set; }
        public string LocationTitle { get; set; }
        public string imgUrl { get; set; }
        public string PresetTitle { get; set; }

    }
}
