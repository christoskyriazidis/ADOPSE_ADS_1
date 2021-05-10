using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class ReportAd
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AdId { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 1)]
        public string ReportText { get; set; }
    }
}
