using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class CreateAd
    {
        public string NewImg { set; get; }
        public IFormFile Img { set; get; }

        [StringLength(200, MinimumLength = 15)]
        [Required]
        public string Title { get; set; }

        [StringLength(250, MinimumLength = 20)]
        [Required]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Type { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Category { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int SubCategoryId { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Condition { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int Manufacturer { get; set; }

        [Range(1, 1000000)]
        [Required]
        public int Price { get; set; }
        public int State { get; set; }
        public string LastUpdate { get; set; }
        public string CreateDate { get; set; }
        public int Customer { get; set; }
    }
}
