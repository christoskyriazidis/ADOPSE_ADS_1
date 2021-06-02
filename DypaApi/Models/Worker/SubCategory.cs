using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Worker
{
    public class SubCategory
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        public int OptimalSoilMoisture { get; set; }

        [Required]
        public int LowestNormalSoilMoisture { get; set; }

        [Required]
        public int UpperNormalSoilMoisture { get; set; }

        [Required]
        public int WeeklyRootWaterWinter { get; set; }

        [Required]
        public int WeeklyRootWaterSummer { get; set; }

    }
}
