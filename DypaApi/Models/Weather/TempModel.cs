using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Weather
{
    public class TempModel
    {
        public float day { get; set; }
        public decimal min { get; set; }
        public decimal max { get; set; }
        public float night { get; set; }
        public float eve { get; set; }
        public float morn { get; set; }
    }
}
