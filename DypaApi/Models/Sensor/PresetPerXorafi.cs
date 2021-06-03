using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Sensor
{
    public class PresetPerXorafi
    {
        public int Id { get; set; }
        public int XorafiId { get; set; }
        public int PresetId { get; set; }
    }
}
