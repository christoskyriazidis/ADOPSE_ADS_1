using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.PostRequest
{
    public class SensorToXorafi
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SensorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int XorafiId { get; set; }
    }
}
