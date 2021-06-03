using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.ElasticSearch
{
    public class ElasticXorafi
    {
        public string region { get; set; }
        public string coords { get; set; }
        public string timestamp { get; set; }
        public decimal wind_deg { get; set; }
        public decimal humidity { get; set; }
        public decimal temp { get; set; }
        public decimal wind_speed { get; set; }
        public string pressure { get; set; }
        public string visibility { get; set; }
    }
}
